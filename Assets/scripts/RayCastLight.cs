using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class RayCastLight : MonoBehaviour
{
    public Transform originLeft;
    public Transform originRight;
    public Transform targetLeft;
    public Transform targetRight;
    
    public int raysCount = 4;
    public bool debuger;
    public bool is2D;

    private List<Vector3> originVectors = new List<Vector3>();
    private List<Vector3> targetVectors = new List<Vector3>();
    private Vector3[] vertices;
    private Mesh mesh;


    void Start()
    {
        updateVectors();
        calculateRayCasts();
        vertices = originVectors.Concat(targetVectors).ToArray();
        generateMesh();
    }

    private void Update()
    {
        updateVectors();
        calculateRayCasts();
        vertices = originVectors.Concat(targetVectors).ToArray();

        mesh.vertices = vertices;
        mesh.triangles = getMeshTriangles();
        mesh.uv = getUVs();

    }

    private void calculateRayCasts()
    {
        for (int i = 0; i < originVectors.Count; i++)
        {
            float distance = Vector3.Distance(originVectors[i], targetVectors[i]);
            RaycastHit hit;
            Vector3 direction = transform.TransformPoint(targetVectors[i]) - transform.TransformPoint(originVectors[i]);

            if (is2D)
            {
                RaycastHit2D hitInfo = Physics2D.Raycast(transform.TransformPoint(originVectors[i]), direction, distance);
                if (hitInfo)
                {
                    targetVectors[i] = transform.InverseTransformPoint(hitInfo.point);
                }
            }
            else
            {
                if (Physics.Raycast(transform.TransformPoint(originVectors[i]), direction, out hit, distance))
                {
                    targetVectors[i] = transform.InverseTransformPoint(hit.point);
                }
            }
            
        }
    }

    private Vector2[] getUVs()
    {
        Vector2[] uvs = new Vector2[vertices.Length];

        
        for (int i = 0; i < uvs.Length; i++)
        {
            if (i < uvs.Length / raysCount)
            {
                uvs[i] = new Vector2(i * (1f / raysCount), 0);                
            }
            else
            {
                float uvSubstraction = 1f / raysCount * (raysCount + 1);
                float uvIndex = 1f / raysCount;
                
                uvs[i] = new Vector2(i * uvIndex - uvSubstraction , 1);                                
            }
        }

        return uvs;
    }
    
    private void updateVectors()
    {
        originVectors = calculateVectors(
            transform.InverseTransformPoint(originLeft.position),
            transform.InverseTransformPoint(originRight.position)
            );
        targetVectors = calculateVectors(
            transform.InverseTransformPoint(targetLeft.position),
            transform.InverseTransformPoint(targetRight.position)
            );
    }
    
    private void generateMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = getMeshTriangles();
        mesh.uv = getUVs();


    }

    private int[] getMeshTriangles()
    {
        int[] triangles = new int[raysCount * 6];
        int tri = 0;
        for (int i = 0; i < raysCount; i++)
        {
            triangles[tri + 0] = i + 0;
            triangles[tri + 1] = i + 1 + raysCount;
            triangles[tri + 2] = i + 1;

            triangles[tri + 3] = i + 1;
            triangles[tri + 4] = i + 1 + raysCount;
            triangles[tri + 5] = i + 1 + raysCount + 1;
            tri += 6;
        }

        return triangles;
    }

    private List<Vector3> calculateVectors(Vector3 start, Vector3 end)
    {
        List<Vector3> vectors = new List<Vector3>();
        vectors.Add(start);

        for (int i = 1; i <= raysCount; i++)
        {
            float distance = Vector3.Distance(start, end);
            Vector3 raylocalPosition =
                start + (end - start).normalized * distance / raysCount * i;
            vectors.Add(raylocalPosition);
        }

        return vectors;
    }
    
    private void OnDrawGizmos()
    {
        if (debuger)
        {
            createHelperGizmos();
        }
    }
    
    private void createHelperGizmos()
    {
        Gizmos.DrawRay(originLeft.position, targetLeft.position - originLeft.position);
        Gizmos.DrawRay(originRight.position, targetRight.position - originRight.position);
        
        if (vertices == null)
        {
            return;
        }


        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawWireSphere(transform.TransformPoint(vertices[i]), .1f);
        }
    }
}