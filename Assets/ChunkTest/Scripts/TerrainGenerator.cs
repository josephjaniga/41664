using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainGenerator : MonoBehaviour {

	public GameObject chunk;
	public GameObject polyChunk;

	public float chunkSize;

	public int currentChunkX = 0;
	public int currentChunkY = 0;

	public Pool polyChunkPool;

	public Stack<Vector2> chunkQueue = new Stack<Vector2>();
	public bool isRunning = false;

	public int maxLODRange = 8;
	public int lastLOD = 0;

	// Use this for initialization
	void Start () {

		polyChunkPool = GameObject.Find("ChunkPool").GetComponent<Pool>();
		PolyChunk vc = polyChunk.GetComponent<PolyChunk>();
		vc.chunkSize = vc.chunkRes;
		chunkSize = vc.chunkSize;
		
	}
	
	// Update is called once per frame
	void Update () {

		float tempX = currentChunkX * chunkSize;
		float tempY = currentChunkY * chunkSize;

		// process the chunk loading queue
		if ( !isRunning ) StartCoroutine(processQueueCoroutine());

		// moving down
		if ( transform.position.y <= tempY - chunkSize/2f ){
			currentChunkY--;
			tempX = currentChunkX * chunkSize;
			tempY = currentChunkY * chunkSize;

			renderRadius(tempX, tempY);
			lastLOD = 0;
		}

		// moving up
		if ( transform.position.y >= tempY + chunkSize/2f ){
			currentChunkY++;
			tempX = currentChunkX * chunkSize;
			tempY = currentChunkY * chunkSize;

			renderRadius(tempX, tempY);
			lastLOD = 0;
		}

		// moving right
		if ( transform.position.x >= tempX + chunkSize/2f ){
			currentChunkX++;
			tempX = currentChunkX * chunkSize;
			tempY = currentChunkY * chunkSize;

			renderRadius(tempX, tempY);
			lastLOD = 0;
		}

		// moving left
		if ( transform.position.x <= tempX - chunkSize/2f ){
			currentChunkX--;
			tempX = currentChunkX * chunkSize;
			tempY = currentChunkY * chunkSize;

			renderRadius(tempX, tempY);
			lastLOD = 0;
		}
	}

	public void chunkFromPool(float x, float y){
		if ( polyChunkPool.getChunkAtPosition(x, y) == null ){
			GameObject temp = polyChunkPool.popFromStack();
			if ( temp != null ) {
				temp.transform.position = new Vector3(x, y, 0f);
				temp.GetComponent<PolyChunk>().updateChunk();
				temp.GetComponent<PolyChunk>().inUse = true;
			}
		}
	}

	public void renderRadius(float x, float y, int r=3){
		// 		x  x  x  x  x
		// 		x  x  x  x  x
		// 		x  x  O  x  x
		// 		x  x  x  x  x
		// 		x  x  x  x  x
		for ( int i=-r; i<=r; i++ ){
			for ( int j=-r; j<=r; j++ ){
				if ( !(i==r || i==-r) && !(j==r || j==-r) )
					queueChunk(x+i*chunkSize,y+j*chunkSize);
					//chunkFromPool(x+i*chunkSize,y+j*chunkSize);
			}			
		}
	}

	public void queueChunk(float x, float y){
		if ( polyChunkPool.getChunkAtPosition(x, y) == null ){
			// queue this chunk
			chunkQueue.Push(new Vector2(x, y));
		}
	}

	IEnumerator processQueueCoroutine(){
		while ( isRunning ) yield return null;
		isRunning = true;
		if ( chunkQueue.Count > 0 ){
			Vector2 temp = chunkQueue.Pop();
			chunkFromPool(temp.x, temp.y);
		} else {
			if ( lastLOD+1 < maxLODRange ){
				lastLOD++;
				renderRadius(currentChunkX * chunkSize, currentChunkY * chunkSize, lastLOD);
			}
		}
		isRunning = false;
	}

}
