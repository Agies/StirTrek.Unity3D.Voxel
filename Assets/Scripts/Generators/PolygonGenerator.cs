using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PolygonGenerator : MonoBehaviour
{

    protected List<Vector3> newVertices = new List<Vector3>();
    protected List<int> newTriangles = new List<int>();
    protected List<Vector2> newUV = new List<Vector2>();
    private Mesh mesh;

    private float textureUnit = 0.25f;
    public Vector2 chosenTexture;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;

        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;

        newVertices.Add(new Vector3(x, y, z));
        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x + 1, y - 1, z));
        newVertices.Add(new Vector3(x, y - 1, z));

        newTriangles.Add(0);
        newTriangles.Add(1);
        newTriangles.Add(3);
        newTriangles.Add(1);
        newTriangles.Add(2);
        newTriangles.Add(3);

        newUV.Add(new Vector2(textureUnit * chosenTexture.x, textureUnit * chosenTexture.y + textureUnit));
        newUV.Add(new Vector2(textureUnit * chosenTexture.x + textureUnit, textureUnit * chosenTexture.y + textureUnit));
        newUV.Add(new Vector2(textureUnit * chosenTexture.x + textureUnit, textureUnit * chosenTexture.y));
        newUV.Add(new Vector2(textureUnit * chosenTexture.x, textureUnit * chosenTexture.y));

        mesh.Clear();
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();
    }

    void Update()
    {
        newUV.Clear();
        newUV.Add(new Vector2(textureUnit * chosenTexture.x, textureUnit * chosenTexture.y + textureUnit));
        newUV.Add(new Vector2(textureUnit * chosenTexture.x + textureUnit, textureUnit * chosenTexture.y + textureUnit));
        newUV.Add(new Vector2(textureUnit * chosenTexture.x + textureUnit, textureUnit * chosenTexture.y));
        newUV.Add(new Vector2(textureUnit * chosenTexture.x, textureUnit * chosenTexture.y));

        mesh.Clear();
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();
    }
}

