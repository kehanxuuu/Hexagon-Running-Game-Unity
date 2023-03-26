using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerate : MonoBehaviour
{
    public float lowest_height;
    public float ratio = 1.7f;
    public int height = 50;
    public int width = 200;
    public float PerlinNoiseXZRatio = 3.1f;
    public float PerlinNoiseYRatio = 1.3f;

    public Gradient gradient;
    float maxHeight = float.MinValue;
    float minHeight = float.MaxValue;

    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    MeshCollider meshCollider;

    // 用来存放顶点数据
    List<Vector3> verts;
    List<int> indices;
    List<Color> colors;

    private void Awake()
    {
        verts = new List<Vector3>();
        indices = new List<int>();
        colors = new List<Color>();

        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();

       // Generate();
    }

    /*void Update()
    {
        Generate();
    }*/

    public void Generate()
    {
        ClearMeshData();

        // 把数据填写好
        AddMeshData();

        // 把数据传递给Mesh，生成真正的网格
        Mesh mesh = new Mesh();
        mesh.vertices = verts.ToArray();
        //mesh.uv = uvs.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.colors = colors.ToArray();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter.mesh = mesh;
        // 碰撞体专用的mesh，只负责物体的碰撞外形
        meshCollider.sharedMesh = mesh;
    }

    void ClearMeshData()
    {
        verts.Clear();
        indices.Clear();
        colors.Clear();
    }

    void AddMeshData()
    {
        float y = 0;
        for (int z=0; z<width; z++)
        {
            for (int x=0; x<height; x++)
            {
                y = 0;
                //for (int i=0; i<8; i++)
                for (int i=0; i<1; i++)
                {
                    //float y = Random.Range(0, 3.5f);
                    y += Mathf.PerlinNoise(x*PerlinNoiseXZRatio, z*PerlinNoiseXZRatio)*PerlinNoiseYRatio;
                    //PerlinNoiseXZRatio *= 2;
                    //PerlinNoiseYRatio *= 0.5f;
                }
                y = Mathf.Max(y, lowest_height);
                //y = Mathf.Min(y, lowest_height+0.405);

                Scene.hexNum hexN = Hexagon.AxisToNum(x*ratio+transform.position.x, z*ratio+transform.position.z);
                Scene.hexNum[] neighbour = new Scene.hexNum[] {hexN};
                Utils.Add(ref neighbour, Hexagon.GetNeighbourHex(hexN, OutOfBound:false));

                for (int i=0; i<neighbour.Length; i++)
                {
                    int num = Scene.GetOneDimensionVal(neighbour[i].num_x, neighbour[i].num_z);
                    if (num>=0 && num<Scene.hexOccupied.Length && Scene.hexOccupied[num])
                    {
                        //Utils.DebugDrawPoint(hexN, 0, Color.blue, y:lowest_height+0.35f+transform.position.y);
                        if (Scene.map[num].name == "jump(Clone)")
                            y = Mathf.Min(y, lowest_height+0.1f);
                        else y = Mathf.Min(y, lowest_height+0.35f);
                        break;
                    }
                }

                if (y > maxHeight) maxHeight = y;
                if (y < minHeight) minHeight = y;

                Vector3 p = new Vector3(x*ratio, y, z*ratio);
                verts.Add(p);
            }
        }

        for (int z=0; z<width; z++)
        {
            for (int x=0; x<height; x++)
            {
                int index = z*height + x;
                y = verts[index].y;
                float p = (y-minHeight)/(maxHeight-minHeight);
                Color c = gradient.Evaluate(p);
                colors.Add(c);
            }
        }

        for (int z=0; z<width-1; z++)
        {
            for (int x=0; x<height-1; x++)
            {
                int index = z*height + x;
                int index1 = (z+1)*height + x;
                int index2 = (z+1)*height + x+1;
                int index3 = z*height + x+1;

                indices.Add(index); indices.Add(index1); indices.Add(index2);
                indices.Add(index); indices.Add(index2); indices.Add(index3);
            }
        }
    }
    
}
