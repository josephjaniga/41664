using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class PolygonGenerator : MonoBehaviour {

 	public List<Vector3> newVertices = new List<Vector3>();
 	public List<int> newTriangles = new List<int>();
 	public List<Vector2> newUV = new List<Vector2>();
 	
 	private Mesh mesh;
 	
 	private float tUnit = 0.25f;
 	private Vector2 tStone = new Vector2 (1, 0);
 	private Vector2 tGrass = new Vector2 (0, 1);
 	private Vector2 tSand = new Vector2 (2, 2);

 	private int squareCount;

 	public byte[,] blocks;


	public List<Vector3> colVertices = new List<Vector3>();
	public List<int> colTriangles = new List<int>();
	private int colCount;

	private MeshCollider col;

	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshFilter>().mesh;

		col = GetComponent<MeshCollider> ();

		float x = transform.position.x;
		float y = transform.position.y;
		float z = transform.position.z;

		GenTerrain();
		BuildMesh();
		UpdateMesh();
	}
	
	// Update is called once per frame
	void Update () {

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
		newVertices.Add( new Vector3 (x  , y  , 0 ));
		newVertices.Add( new Vector3 (x + 1 , y  , 0 ));
		newVertices.Add( new Vector3 (x + 1 , y-1 , 0 ));
		newVertices.Add( new Vector3 (x  , y-1 , 0 ));
		  
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
		blocks=new byte[128,128];

		for(int px=0;px<blocks.GetLength(0);px++){
			for(int py=0;py<blocks.GetLength(1);py++){
				Vector3 thePosition = new Vector3((float)px + transform.position.x, (float)py + transform.position.y, 0f);

				if ( Noise.valueAtPoint(thePosition, 0.1f, 2) > 0.5f ){
					blocks[px,py]=3;
				} else {
					blocks[px,py]=0;
				}

				if ( Noise.valueAtPoint(thePosition, 0.1f, 2) > 0.75f ){
					blocks[px,py]=2;
				} 

				// if(py==5){
				// 	blocks[px,py]=3;
				// } else if(py<5){
				// 	blocks[px,py]=1;
				// }
			}
		}
	}

	void BuildMesh(){
		for(int px=0;px<blocks.GetLength(0);px++){
			for(int py=0;py<blocks.GetLength(1);py++){
				if(blocks[px,py]!=0){
					GenCollider(px,py);
					if(blocks[px,py]==1){
						GenSquare(px,py,tStone);
					} else if(blocks[px,py]==2){
						GenSquare(px,py,tGrass);
					} else if(blocks[px,py]==3){
						GenSquare(px,py,tSand);
					}
				}
			}
		}
	}

	void GenCollider(int x, int y){
		//Top
		if(Block(x,y+1)==0){
			colVertices.Add( new Vector3 (x  , y  , 1));
			colVertices.Add( new Vector3 (x + 1 , y  , 1));
			colVertices.Add( new Vector3 (x + 1 , y  , 0 ));
			colVertices.Add( new Vector3 (x  , y  , 0 ));

			ColliderTriangles();

			colCount++;
		}

		//bot
		if(Block(x,y-1)==0){
			colVertices.Add( new Vector3 (x  , y -1 , 0));
			colVertices.Add( new Vector3 (x + 1 , y -1 , 0));
			colVertices.Add( new Vector3 (x + 1 , y -1 , 1 ));
			colVertices.Add( new Vector3 (x  , y -1 , 1 ));

			ColliderTriangles();
			colCount++;
		}

		//left
		if(Block(x-1,y)==0){
			colVertices.Add( new Vector3 (x  , y -1 , 1));
			colVertices.Add( new Vector3 (x  , y  , 1));
			colVertices.Add( new Vector3 (x  , y  , 0 ));
			colVertices.Add( new Vector3 (x  , y -1 , 0 ));

			ColliderTriangles();

			colCount++;
		}

		//right
		if(Block(x+1,y)==0){
			colVertices.Add( new Vector3 (x +1 , y  , 1));
			colVertices.Add( new Vector3 (x +1 , y -1 , 1));
			colVertices.Add( new Vector3 (x +1 , y -1 , 0 ));
			colVertices.Add( new Vector3 (x +1 , y  , 0 ));

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
