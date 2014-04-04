﻿using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PolygonGenerator : MonoBehaviour
{

    protected List<Vector3> newVertices = new List<Vector3>();
    protected List<int> newTriangles = new List<int>();
    protected List<Vector2> newUV = new List<Vector2>();
    private Mesh mesh;

    private float textureUnit = 0.25f;
    
    public int squareCount;
    
    private Vector2?[,] blocks;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        GenerateTerrain();
        BuildMesh();
        UpdateMesh();
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();

        squareCount = 0;
        newVertices.Clear();
        newTriangles.Clear();
        newUV.Clear();
    }

    private void BuildMesh()
    {
        for (int px = 0; px < blocks.GetLength(0); px++)
        {
            for (int py = 0; py < blocks.GetLength(1); py++)
            {
                GenerateSquare(px, py, blocks[px, py]);
            }
        }
    }

    private void GenerateSquare(int x, int y, Vector2? tex)
    {
        if (tex == null) return;
        var texture = tex.Value;
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

    private void GenerateTerrain()
    {
        blocks = new Vector2?[20,8];
        for (int px = 0; px < blocks.GetLength(0); px++)
        {
            for (int py = 0; py < blocks.GetLength(1); py++)
            {
                if (py == 5)
                {
                    blocks[px, py] = new Vector2(1, Random.Range(0,4));
                }
                else if (py < 5)
                {
                    blocks[px, py] = new Vector2(0, Random.Range(0, 4));
                }
            }
        }
    }

    void Update()
    {
        
    }
}

