using UnityEngine;
using System.Collections;

public class VoxelChunk : MonoBehaviour {

	public GameObject voxel;
	public float voxelSize;
	public int chunkRes = 16;
	public float chunkSize;

	public Pool voxelPool;

	void Update () {
		if ( GameObject.Find("VoxelPool") != null ){
			voxelPool = GameObject.Find("VoxelPool").GetComponent<Pool>();
		}
	}

	void Awake(){
		if ( GameObject.Find("VoxelPool") != null ){
			voxelPool = GameObject.Find("VoxelPool").GetComponent<Pool>();
		}
	}

	// Use this for initialization
	void Start () {

		if ( GameObject.Find("VoxelPool") != null ){
			voxelPool = GameObject.Find("VoxelPool").GetComponent<Pool>();
		}

		transform.localScale = new Vector3(chunkSize, chunkSize, chunkSize);

		GameObject voxels = GameObject.Find("Voxels");
		if ( voxels == null ){
			voxels = new GameObject("Voxels");
		}

		// make all voxels withing size
		Vector3 thePosition = Vector3.zero;
		for (int x=-16; x<16; x++){
			for(int y=-16; y<16; y++){
				thePosition = new Vector3(transform.position.x + (float)x * voxelSize, transform.position.y + (float)y * voxelSize, 0f);
				if ( Noise.valueAtPoint(thePosition, 0.1f, 2) > 0.5f ){
					GameObject temp = Instantiate(voxel, thePosition, Quaternion.identity) as GameObject;
					temp.transform.SetParent(transform);
					// GameObject temp = voxelPool.getFirstAvailable();
					// temp.SetActive(true);
					// temp.transform.SetParent(transform);
					// temp.transform.position = thePosition;
				}
			}
		}

	}

	public void unload(){
		// foreach ( Transform child in transform ) {
		// 	voxelPool.pushToStack(child.gameObject);
		// }
		// voxelPool.countAvailable();
		Destroy(gameObject);
	}


}
