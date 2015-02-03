using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class PolyChunk : MonoBehaviour {

	public bool inUse = false;
	public float startTime;

	public int chunkRes = 32;
	public float chunkSize;

 	public List<Vector3> newVertices = new List<Vector3>();
 	public List<int> newTriangles = new List<int>();
 	public List<Vector2> newUV = new List<Vector2>();
 	
 	private Mesh mesh;
 	
 	private float tUnit = 0.25f;
	// 1 2 3 
	// 4 5 6
	// 7 8 9

 	private static Vector2 tStone;
 	private static Vector2 tGrass;
 	private static Vector2 tSand;

 	private static Vector2 topLeft 		= new Vector2 (0, 3); // 1
 	private static Vector2 topCenter 	= new Vector2 (1, 3); // 2
 	private static Vector2 topRight 	= new Vector2 (2, 3); // 3
 	private static Vector2 midLeft 		= new Vector2 (0, 2); // 4
 	private static Vector2 midCenter 	= new Vector2 (1, 2); // 5
 	private static Vector2 midRight 	= new Vector2 (2, 2); // 6
 	private static Vector2 botLeft 		= new Vector2 (0, 1); // 7
 	private static Vector2 botCenter 	= new Vector2 (1, 1); // 8
 	private static Vector2 botRight 	= new Vector2 (2, 1); // 9

 	private int squareCount;

 	public byte[,] blocks;

	public List<Vector3> colVertices = new List<Vector3>();
	public List<int> colTriangles = new List<int>();
	private int colCount;

	private MeshCollider col;

	// Use this for initialization
	void Start () {

 		tStone = new Vector2 (1, Random.Range(0,3));
 		tGrass = new Vector2 (2, Random.Range(0,3));
 		tSand = new Vector2 (3, Random.Range(0,3));

		startTime = Time.time;
		mesh = GetComponent<MeshFilter>().mesh;
		col = GetComponent<MeshCollider> ();
		// float x = transform.position.x;
		// float y = transform.position.y;
		// float z = transform.position.z;

		// GenTerrain();
		// BuildMesh();
		// UpdateMesh();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void updateChunk(){
		startTime = Time.time;
		mesh = GetComponent<MeshFilter>().mesh;
		col = GetComponent<MeshCollider> ();
		GenTerrain();
		BuildMesh();
		UpdateMesh();
	}

	void UpdateMesh() {
		mesh.Clear();
		mesh.vertices = newVertices.ToArray();
		mesh.triangles = newTriangles.ToArray();
		mesh.uv = newUV.ToArray();
		mesh.Optimize();
		mesh.RecalculateNormals();

		squareCount=0;
		newVertices.Clear();
		newTriangles.Clear();
		newUV.Clear();

		Mesh newMesh = new Mesh();
		newMesh.vertices = colVertices.ToArray();
		newMesh.triangles = colTriangles.ToArray();
		col.sharedMesh = newMesh;
		 
		colVertices.Clear();
		colTriangles.Clear();
		colCount = 0;
	}

	void GenSquare(int x, int y, Vector2 texture){
		newVertices.Add( new Vector3 (x - chunkRes/2, 		y - chunkRes/2  , 		0 ));
		newVertices.Add( new Vector3 (x - chunkRes/2 + 1, 	y - chunkRes/2  ,		0 ));
		newVertices.Add( new Vector3 (x - chunkRes/2 + 1, 	y - chunkRes/2 - 1 , 	0 ));
		newVertices.Add( new Vector3 (x - chunkRes/2, 		y - chunkRes/2 - 1 , 	0 ));
		  
		newTriangles.Add(squareCount*4);
		newTriangles.Add((squareCount*4)+1);
		newTriangles.Add((squareCount*4)+3);
		newTriangles.Add((squareCount*4)+1);
		newTriangles.Add((squareCount*4)+2);
		newTriangles.Add((squareCount*4)+3);
		  
		newUV.Add(new Vector2 (tUnit * texture.x, tUnit * texture.y + tUnit));
		newUV.Add(new Vector2 (tUnit*texture.x+tUnit, tUnit*texture.y+tUnit));
		newUV.Add(new Vector2 (tUnit * texture.x + tUnit, tUnit * texture.y));
		newUV.Add(new Vector2 (tUnit * texture.x, tUnit * texture.y));
		  
		squareCount++;
	}

	void GenTerrain(){
		blocks=new byte[chunkRes,chunkRes];
		// air - 0    ground - 1

		for(int px=0;px<blocks.GetLength(0);px++){
			for(int py=0;py<blocks.GetLength(1);py++){
				Vector3 thePosition = new Vector3((float)px + transform.position.x, (float)py + transform.position.y, 0f);

				if ( Noise.valueAtPoint(thePosition, .0125f, 2) >= 0.5f ){
					blocks[px,py]=1;
				} else {
					blocks[px,py]=0;
				}

				// //differentiate air and ground
				// if ( Noise.valueAtPoint(thePosition, .025f, 2) >= 0.5f ){
				// 	// something else
				// 	if ( Noise.valueAtPoint(thePosition, .025f, 2) >= 0.5f && Noise.valueAtPoint(thePosition, 0.1f, 2) <= 0.6f ){
				// 		blocks[px,py]=1;
				// 	} else {
				// 		blocks[px,py]=3;
				// 	}
				// } else {
				// 	blocks[px,py]=0;
				// }

				// // dark ground
				// if ( Noise.valueAtPoint(thePosition, .025f, 2) > 0.75f ){
				// 	blocks[px,py]=2;
				// } 

			}
		}
	}

	void BuildMesh(){
		for(int px=0;px<blocks.GetLength(0);px++){
			for(int py=0;py<blocks.GetLength(1);py++){
				if(blocks[px,py]!=0){
					GenCollider(px,py);

					// positions
					// 1 2 3 
					// 4 5 6
					// 7 8 9

					// if this block is solid
					if ( blocks[px,py]==1 ){

						// top left 1
						// up and left are air
						if ( Block(px,py+1) == 0 && Block(px-1,py) == 0){
							GenSquare(px,py,topLeft);
						}

						// top center 2
						// up is air, right and left are solid
						else if ( Block(px,py+1) == 0 && Block(px-1,py) == 1 && Block(px+1,py) == 1 ){
							GenSquare(px,py,topCenter);
						}

						// top right 3
						// up and right are air
						else if ( Block(px,py+1) == 0 && Block(px+1,py) == 0){
							GenSquare(px,py,topRight);
						}

						// mid left 4
						// left air
						// up and down solid
						else if ( Block(px-1,py) == 0 && Block(px,py+1) == 1 && Block(px,py-1) == 1){
							GenSquare(px,py,midLeft);
						}

						// mid center 5
						// up and down solid
						else if ( Block(px-1,py) == 1 && Block(px+1,py) == 1 && Block(px,py-1) == 1 && Block(px,py+1) == 1 ){
							GenSquare(px,py,midCenter);
						}

						// mid right 6
						// up and down solid
						else if ( Block(px-1,py) == 1 && Block(px+1,py) == 0 && Block(px,py-1) == 1 && Block(px,py+1) == 1 ){
							GenSquare(px,py,midCenter);
						}

						// bot left 7
						// left bottom air
						else if ( Block(px-1,py) == 0 && Block(px,py-1) == 0 ){
							GenSquare(px,py,botLeft);
						}

						// bot center 8
						// left bottom air
						else if ( Block(px-1,py) == 1 && Block(px+1,py) == 1 && Block(px,py-1) == 0 ){
							GenSquare(px,py,botCenter);
						}

						else {
							GenSquare(px,py,botRight);
						}

					}



					// if(blocks[px,py]==1){
					// 	GenSquare(px,py,tStone);
					// } else if(blocks[px,py]==2){
					// 	GenSquare(px,py,tGrass);
					// } else if(blocks[px,py]==3){
					// 	GenSquare(px,py,tSand);
					// }
				}
			}
		}
	}

	void GenCollider(int x, int y){
		//Top
		if(Block(x,y+1)==0){
			colVertices.Add( new Vector3 (x - chunkRes/2, 		y - chunkRes/2, 1));
			colVertices.Add( new Vector3 (x - chunkRes/2 + 1, 	y - chunkRes/2, 1));
			colVertices.Add( new Vector3 (x - chunkRes/2 + 1, 	y - chunkRes/2, 0 ));
			colVertices.Add( new Vector3 (x - chunkRes/2, 		y - chunkRes/2, 0 ));

			ColliderTriangles();

			colCount++;
		}

		//bot
		if(Block(x,y-1)==0){
			colVertices.Add( new Vector3 (x - chunkRes/2, 		y - chunkRes/2 - 1 , 0 ));
			colVertices.Add( new Vector3 (x - chunkRes/2 + 1, 	y - chunkRes/2 - 1 , 0 ));
			colVertices.Add( new Vector3 (x - chunkRes/2 + 1, 	y - chunkRes/2 - 1 , 1 ));
			colVertices.Add( new Vector3 (x - chunkRes/2, 		y - chunkRes/2 - 1 , 1 ));

			ColliderTriangles();
			colCount++;
		}

		//left
		if(Block(x-1,y)==0){
			colVertices.Add( new Vector3 (x - chunkRes/2, y - chunkRes/2 -1,	1 ));
			colVertices.Add( new Vector3 (x - chunkRes/2, y - chunkRes/2, 		1 ));
			colVertices.Add( new Vector3 (x - chunkRes/2, y - chunkRes/2, 		0 ));
			colVertices.Add( new Vector3 (x - chunkRes/2, y - chunkRes/2 -1,	0 ));

			ColliderTriangles();

			colCount++;
		}

		//right
		if(Block(x+1,y)==0){
			colVertices.Add( new Vector3 (x - chunkRes/2 + 1, y - chunkRes/2, 		1 ));
			colVertices.Add( new Vector3 (x - chunkRes/2 + 1, y - chunkRes/2 - 1, 	1 ));
			colVertices.Add( new Vector3 (x - chunkRes/2 + 1, y - chunkRes/2 - 1, 	0 ));
			colVertices.Add( new Vector3 (x - chunkRes/2 + 1, y - chunkRes/2,		0 ));

			ColliderTriangles();

			colCount++;
		}
	}

	void ColliderTriangles(){
		colTriangles.Add(colCount*4);
		colTriangles.Add((colCount*4)+1);
		colTriangles.Add((colCount*4)+3);
		colTriangles.Add((colCount*4)+1);
		colTriangles.Add((colCount*4)+2);
		colTriangles.Add((colCount*4)+3);
	}

	byte Block (int x, int y){
		if ( x == -1 || x == blocks.GetLength(0) || y == -1 || y == blocks.GetLength(1) ){
			return (byte)1;
		}
		return blocks[x,y];
	}


}
