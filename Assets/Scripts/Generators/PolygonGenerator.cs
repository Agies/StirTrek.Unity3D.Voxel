using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PolygonGenerator : MonoBehaviour
{

    private List<Vector3> newVertices = new List<Vector3>();
    private List<int> newTriangles = new List<int>();
    private List<Vector2> newUV = new List<Vector2>();
    
    private List<Vector3> colVertices = new List<Vector3>();
    private List<int> colTriangles = new List<int>();
    private int colCount;

    private Mesh mesh;
    private MeshCollider col;

    private float textureUnit = 0.25f;
    private Vector2 textureDirt = new Vector2(0, 3);
    private Vector2 textureGrass = new Vector2(1, 3);

    public byte[,] blocks;
    private int squareCount;
    
    // Use this for initialization
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        col = GetComponent<MeshCollider>();

        GenerateTerrain();
        BuildMesh();
        UpdateMesh();
    }

    void Update()
    {


    }

    void GenerateTerrain()
    {
        blocks = new byte[96, 128];

        for (int px = 0; px < blocks.GetLength(0); px++)
        {
            int stone = Noise(px, 0, 80, 15, 1);
            stone += Noise(px, 0, 50, 30, 1);
            stone += Noise(px, 0, 10, 10, 1);
            stone += 75;

            int dirt = Noise(px, 0, 100, 35, 1);
            dirt += Noise(px, 0, 50, 30, 1);
            dirt += 75;

            for (int py = 0; py < blocks.GetLength(1); py++)
            {
                if (py < stone)
                {
                    blocks[px, py] = 1;
                }
                else if (py < dirt)
                {
                    blocks[px, py] = 2;
                }
            }
        }
    }

    int Noise(int x, int y, float scale, float mag, float exp)
    {
        return (int)(Mathf.Pow((Mathf.PerlinNoise(x / scale, y / scale) * mag), (exp)));
    }

    void BuildMesh()
    {
        for (int px = 0; px < blocks.GetLength(0); px++)
        {
            for (int py = 0; py < blocks.GetLength(1); py++)
            {

                if (blocks[px, py] != 0)
                {

                    GenerateCollider(px, py);

                    if (blocks[px, py] == 1)
                    {
                        GenerateSquare(px, py, textureDirt);
                    }
                    else if (blocks[px, py] == 2)
                    {
                        GenerateSquare(px, py, textureGrass);
                    }
                }
            }
        }
    }

    byte Block(int x, int y)
    {

        if (x == -1 || x == blocks.GetLength(0) || y == -1 || y == blocks.GetLength(1))
        {
            return (byte)1;
        }

        return blocks[x, y];
    }

    void GenerateCollider(int x, int y)
    {

        //Top
        if (Block(x, y + 1) == 0)
        {
            colVertices.Add(new Vector3(x, y, 1));
            colVertices.Add(new Vector3(x + 1, y, 1));
            colVertices.Add(new Vector3(x + 1, y, 0));
            colVertices.Add(new Vector3(x, y, 0));

            ColliderTriangles();

            colCount++;
        }

        //bot
        if (Block(x, y - 1) == 0)
        {
            colVertices.Add(new Vector3(x, y - 1, 0));
            colVertices.Add(new Vector3(x + 1, y - 1, 0));
            colVertices.Add(new Vector3(x + 1, y - 1, 1));
            colVertices.Add(new Vector3(x, y - 1, 1));

            ColliderTriangles();
            colCount++;
        }

        //left
        if (Block(x - 1, y) == 0)
        {
            colVertices.Add(new Vector3(x, y - 1, 1));
            colVertices.Add(new Vector3(x, y, 1));
            colVertices.Add(new Vector3(x, y, 0));
            colVertices.Add(new Vector3(x, y - 1, 0));

            ColliderTriangles();

            colCount++;
        }

        //right
        if (Block(x + 1, y) == 0)
        {
            colVertices.Add(new Vector3(x + 1, y, 1));
            colVertices.Add(new Vector3(x + 1, y - 1, 1));
            colVertices.Add(new Vector3(x + 1, y - 1, 0));
            colVertices.Add(new Vector3(x + 1, y, 0));

            ColliderTriangles();

            colCount++;
        }

    }

    void ColliderTriangles()
    {
        colTriangles.Add(colCount * 4);
        colTriangles.Add((colCount * 4) + 1);
        colTriangles.Add((colCount * 4) + 3);
        colTriangles.Add((colCount * 4) + 1);
        colTriangles.Add((colCount * 4) + 2);
        colTriangles.Add((colCount * 4) + 3);
    }

    void GenerateSquare(int x, int y, Vector2 texture)
    {

        newVertices.Add(new Vector3(x, y, 0));
        newVertices.Add(new Vector3(x + 1, y, 0));
        newVertices.Add(new Vector3(x + 1, y - 1, 0));
        newVertices.Add(new Vector3(x, y - 1, 0));

        newTriangles.Add(squareCount * 4);
        newTriangles.Add((squareCount * 4) + 1);
        newTriangles.Add((squareCount * 4) + 3);
        newTriangles.Add((squareCount * 4) + 1);
        newTriangles.Add((squareCount * 4) + 2);
        newTriangles.Add((squareCount * 4) + 3);

        newUV.Add(new Vector2(textureUnit * texture.x, textureUnit * texture.y + textureUnit));
        newUV.Add(new Vector2(textureUnit * texture.x + textureUnit, textureUnit * texture.y + textureUnit));
        newUV.Add(new Vector2(textureUnit * texture.x + textureUnit, textureUnit * texture.y));
        newUV.Add(new Vector2(textureUnit * texture.x, textureUnit * texture.y));

        squareCount++;

    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();

        newVertices.Clear();
        newTriangles.Clear();
        newUV.Clear();
        squareCount = 0;

        var newMesh = new Mesh
                       {
                           vertices = colVertices.ToArray(), 
                           triangles = colTriangles.ToArray()
                       };
        col.sharedMesh = newMesh;

        colVertices.Clear();
        colTriangles.Clear();
        colCount = 0;
    }
}