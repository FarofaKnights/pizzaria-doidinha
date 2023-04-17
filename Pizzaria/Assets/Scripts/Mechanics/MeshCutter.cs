using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class MeshTriangle {
    public List<Vector3> vertices = new List<Vector3>();
    public List<Vector3> normals = new List<Vector3>();
    public List<Vector2> uvs = new List<Vector2>();
    public int submeshIndex;

    public MeshTriangle(Vector3[] vertices, Vector3[] normals, Vector2[] uvs, int submeshIndex) {
        Clear();

        this.vertices.AddRange(vertices);
        this.normals.AddRange(normals);
        this.uvs.AddRange(uvs);
        this.submeshIndex = submeshIndex;
    }

    public void Clear() {
        vertices.Clear();
        normals.Clear();
        uvs.Clear();

        submeshIndex = 0;
    }
}

public class GeneratedMesh {
    public List<Vector3> vertices = new List<Vector3>();
    public List<Vector3> normals = new List<Vector3>();
    public List<Vector2> uvs = new List<Vector2>();
    public List<List<int>> submeshIndices = new List<List<int>>();

    public void AddTriangle(MeshTriangle triangle) {
        int count = vertices.Count;

        vertices.AddRange(triangle.vertices);
        normals.AddRange(triangle.normals);
        uvs.AddRange(triangle.uvs);

        if (submeshIndices.Count <= triangle.submeshIndex) {
            for (int i = submeshIndices.Count; i <= triangle.submeshIndex; i++) {
                submeshIndices.Add(new List<int>());
            }
        }

        for (int i = 0; i < 3; i++){
            submeshIndices[triangle.submeshIndex].Add(count + i);
        }
    }

    public Mesh GetGeneratedMesh()
    {
        Mesh mesh = new Mesh();
        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);
        mesh.SetUVs(1, uvs);

        mesh.subMeshCount = submeshIndices.Count;
        for (int i = 0; i < submeshIndices.Count; i++)
        {
            mesh.SetTriangles(submeshIndices[i], i);
        }
            return mesh;
    }
}

public class MeshCutter {
    public static bool cutting = false;

    public static GameObject[] CutMesh(GameObject original, Vector3 point, Vector3 direction, Material capMaterial) {
        if (cutting) return null;
        cutting = true;

        Plane plane = new Plane(original.transform.InverseTransformDirection(-direction), original.transform.InverseTransformPoint(point));
        Mesh originalMesh = original.GetComponent<MeshFilter>().mesh;
        List<Vector3> addedVertices = new List<Vector3>();

        GeneratedMesh leftMesh = new GeneratedMesh();
        GeneratedMesh rightMesh = new GeneratedMesh();

        SeparateMeshes(originalMesh, leftMesh, rightMesh, plane, addedVertices);
        FillCut(originalMesh, addedVertices, plane, leftMesh, rightMesh);

        Mesh finishedLeftMesh = leftMesh.GetGeneratedMesh();
        Mesh finishedRightMesh = rightMesh.GetGeneratedMesh();

        //Getting and destroying all original colliders to prevent having multiple colliders
        //of different kinds on one object
        var originalCols = original.GetComponents<Collider>();
        foreach (var col in originalCols)
            Object.Destroy(col);

        original.GetComponent<MeshFilter>().mesh = finishedLeftMesh;
        var collider = original.AddComponent<MeshCollider>();
        collider.sharedMesh = finishedLeftMesh;
        collider.convex = true;
        
        Material[] mats = new Material[finishedLeftMesh.subMeshCount];
        for (int i = 0; i < finishedLeftMesh.subMeshCount; i++) {
            mats[i] = original.GetComponent<MeshRenderer>().material;
        }
        original.GetComponent<MeshRenderer>().materials = mats;


        GameObject right = new GameObject();
        right.transform.position = original.transform.position + (Vector3.up * .05f);
        right.transform.rotation = original.transform.rotation;
        right.transform.localScale = original.transform.localScale;
        right.AddComponent<MeshRenderer>();
        
        mats = new Material[finishedRightMesh.subMeshCount];
        for (int i = 0; i < finishedRightMesh.subMeshCount; i++)
		{
            mats[i] = original.GetComponent<MeshRenderer>().material;
        }
        right.GetComponent<MeshRenderer>().materials = mats;
        right.AddComponent<MeshFilter>().mesh = finishedRightMesh;
        
        right.AddComponent<MeshCollider>().sharedMesh = finishedRightMesh;
        var cols = right.GetComponents<MeshCollider>();
        foreach (var col in cols)
        {
            col.convex = true;
        }

        
        cutting = false;

        return new GameObject[] { original, right };
    }

    public static void SeparateMeshes(Mesh originalMesh,GeneratedMesh leftMesh,GeneratedMesh rightMesh, Plane plane, List<Vector3> addedVertices) {
        int[] submeshIndices;
        int triangleIndexA, triangleIndexB, triangleIndexC;

        for (int submesh = 0; submesh < originalMesh.subMeshCount; submesh++) {
            submeshIndices = originalMesh.GetTriangles(submesh);

            for (int i = 0; i < submeshIndices.Length; i += 3) {
                triangleIndexA = submeshIndices[i];
                triangleIndexB = submeshIndices[i + 1];
                triangleIndexC = submeshIndices[i + 2];

                MeshTriangle currentTriangle = GetTriangle(originalMesh, triangleIndexA, triangleIndexB, triangleIndexC, submesh);

                bool triangleALeftSide = plane.GetSide(originalMesh.vertices[triangleIndexA]);
                bool triangleBLeftSide = plane.GetSide(originalMesh.vertices[triangleIndexB]);
                bool triangleCLeftSide = plane.GetSide(originalMesh.vertices[triangleIndexC]);

                if (triangleALeftSide && triangleBLeftSide && triangleCLeftSide) {
                    leftMesh.AddTriangle(currentTriangle);
                } else if (!triangleALeftSide && !triangleBLeftSide && !triangleCLeftSide) {
                    rightMesh.AddTriangle(currentTriangle);
                } else {
                    CutTriangle(plane, currentTriangle, triangleALeftSide, triangleBLeftSide, triangleCLeftSide, leftMesh, rightMesh, addedVertices);
                }
            }
        }
    }

    private static MeshTriangle GetTriangle(Mesh originalMesh, int _triangleIndexA, int _triangleIndexB, int _triangleIndexC, int _submeshIndex) {
        //Adding the Vertices at the triangleIndex
        Vector3[] verticesToAdd = {
            originalMesh.vertices[_triangleIndexA],
            originalMesh.vertices[_triangleIndexB],
            originalMesh.vertices[_triangleIndexC]
        };

        //Adding the normals at the triangle index
        Vector3[] normalsToAdd = {
            originalMesh.normals[_triangleIndexA],
            originalMesh.normals[_triangleIndexB],
            originalMesh.normals[_triangleIndexC]
        };

        //adding the uvs at the triangleIndex
        Vector2[] uvsToAdd = {
            originalMesh.uv[_triangleIndexA],
            originalMesh.uv[_triangleIndexB],
            originalMesh.uv[_triangleIndexC]
        };

        return new MeshTriangle(verticesToAdd, normalsToAdd, uvsToAdd, _submeshIndex);
    }

    static void CutTriangle(Plane plane, MeshTriangle triangle, bool triangleALeftSide, bool triangleBLeftSide, bool triangleCLeftSide, GeneratedMesh leftSide, GeneratedMesh rightSide, List<Vector3> addedVertices) {
        List<bool> leftSideVertices = new List<bool>();
        leftSideVertices.Add(triangleALeftSide);
        leftSideVertices.Add(triangleBLeftSide);
        leftSideVertices.Add(triangleCLeftSide);

        MeshTriangle leftSideTriangle = new MeshTriangle(new Vector3[2], new Vector3[2], new Vector2[2], triangle.submeshIndex);
        MeshTriangle rightSideTriangle = new MeshTriangle(new Vector3[2], new Vector3[2], new Vector2[2], triangle.submeshIndex);

        bool left = false;
        bool right = false;


        MeshTriangle currentMeshTriangle = null;
        bool currentIsLeft = false;

        for (int i = 0; i< 3; i++) {
            if (leftSideVertices[i]) {
                currentMeshTriangle = leftSideTriangle;
                currentIsLeft = true;
            } else {
                currentMeshTriangle = rightSideTriangle;
                currentIsLeft = false;
            }

            if ((currentIsLeft && !left) || (!currentIsLeft && !right)) {
                if (currentIsLeft) left = true;
                else right = true;
            
                currentMeshTriangle.vertices[0] = triangle.vertices[i];
                currentMeshTriangle.vertices[1] = currentMeshTriangle.vertices[0];

                currentMeshTriangle.normals[0] = triangle.normals[i];
                currentMeshTriangle.normals[1] = currentMeshTriangle.normals[0];

                currentMeshTriangle.uvs[0] = triangle.uvs[i];
                currentMeshTriangle.uvs[1] = currentMeshTriangle.uvs[0];
            } else {
                currentMeshTriangle.vertices[1] = triangle.vertices[i];
                currentMeshTriangle.normals[1] = triangle.normals[i];
                currentMeshTriangle.uvs[1] = triangle.uvs[i];
            }
        }

        float normalizedDistance;
        float distance;

        Vector3[] vert = new Vector3[2];
        Vector3[] norm = new Vector3[2];
        Vector2[] uv = new Vector2[2];

        for (int i = 0; i < 2; i++) {
            plane.Raycast(new Ray(leftSideTriangle.vertices[i], (rightSideTriangle.vertices[i] - leftSideTriangle.vertices[i]).normalized), out distance);

            normalizedDistance = distance / (rightSideTriangle.vertices[i] - leftSideTriangle.vertices[i]).magnitude;
            vert[i] = Vector3.Lerp(leftSideTriangle.vertices[i], rightSideTriangle.vertices[i], normalizedDistance);
            addedVertices.Add(vert[i]);

            norm[i] = Vector3.Lerp(leftSideTriangle.normals[i], rightSideTriangle.normals[i], normalizedDistance);
            uv[i] = Vector2.Lerp(leftSideTriangle.uvs[i], rightSideTriangle.uvs[i], normalizedDistance);
        }

        Vector2 uvLeft = uv[0];
        Vector2 uvRight = uv[1];


        currentMeshTriangle = leftSideTriangle;
        for (int i = 0; i < 2; i++) {
            TestTriangle(
                leftSide,
                new Vector3[] { currentMeshTriangle.vertices[0], vert[0], vert[1] },
                new Vector3[] { currentMeshTriangle.normals[0], norm[0], norm[1] },
                new Vector2[] { currentMeshTriangle.uvs[0], uvLeft, uvRight },
                currentMeshTriangle.submeshIndex
            );

            TestTriangle(
                leftSide,
                new Vector3[] { currentMeshTriangle.vertices[0], currentMeshTriangle.vertices[1], vert[1] },
                new Vector3[] { currentMeshTriangle.normals[0], currentMeshTriangle.normals[1], norm[1] },
                new Vector2[] { currentMeshTriangle.uvs[0], currentMeshTriangle.uvs[1], uvRight },
                currentMeshTriangle.submeshIndex
            );

            currentMeshTriangle = rightSideTriangle;
        }

    }

    static void TestTriangle(GeneratedMesh mesh, Vector3[] updatedVertices, Vector3[] updatedNormals, Vector2[] updatedUVs, int index) {
        MeshTriangle currentTriangle = new MeshTriangle(updatedVertices, updatedNormals, updatedUVs, index);
         if(updatedVertices[0] != updatedVertices[1] && updatedVertices[0] != updatedVertices[2]) {
            if(Vector3.Dot(Vector3.Cross(updatedVertices[1] - updatedVertices[0],updatedVertices[2] - updatedVertices[0]),updatedNormals[0]) < 0) 
                FlipTriangle(currentTriangle);

            mesh.AddTriangle(currentTriangle);
        }
    }

    static void FlipTriangle(MeshTriangle triangle) {
        Vector3 temp = triangle.vertices[2];
        triangle.vertices[2] = triangle.vertices[0];
        triangle.vertices[0] = temp;

        temp = triangle.normals[2];
		triangle.normals[2] = triangle.normals[0];
		triangle.normals[0] = temp;

		(triangle.uvs[2], triangle.uvs[0]) = (triangle.uvs[0], triangle.uvs[2]);
    }

    public static void FillCut(Mesh originalMesh, List<Vector3> addedVertices, Plane plane, GeneratedMesh leftMesh, GeneratedMesh rightMesh) {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> polygon = new List<Vector3>();

        for (int i = 0; i < addedVertices.Count; i++)
        {
            if(!vertices.Contains(addedVertices[i]))
            {
                polygon.Clear();
                polygon.Add(addedVertices[i]);
                polygon.Add(addedVertices[i + 1]);

                vertices.Add(addedVertices[i]);
                vertices.Add(addedVertices[i + 1]);

                EvaluatePairs(addedVertices, vertices, polygon);
                Fill(originalMesh, polygon, plane, leftMesh, rightMesh);
            }
        }
    }

    static void EvaluatePairs(List<Vector3> addedVertices,List<Vector3> vertices, List<Vector3> polygone) {
        bool isDone = false;
        while(!isDone) {
            isDone = true;
            for (int i = 0; i < addedVertices.Count; i+=2) {
                if(addedVertices[i] == polygone[polygone.Count - 1] && !vertices.Contains(addedVertices[i + 1])) {
                    isDone = false;
                    polygone.Add(addedVertices[i + 1]);
                    vertices.Add(addedVertices[i + 1]);
                } 
                else if (addedVertices[i + 1] == polygone[polygone.Count - 1] && !vertices.Contains(addedVertices[i])) {
                    isDone = false;
                    polygone.Add(addedVertices[i]);
                    vertices.Add(addedVertices[i]);
                }
            }
        }
    }

    private static void Fill(Mesh originalMesh, List<Vector3> vertices, Plane plane, GeneratedMesh leftMesh, GeneratedMesh rightMesh) {
        //Firstly we need the center we do this by adding up all the vertices and then calculating the average
        Vector3 centerPosition = Vector3.zero;
        for (int i = 0; i < vertices.Count; i++)
            centerPosition += vertices[i];
        
        centerPosition /= vertices.Count;

        //We now need an Upward Axis we use the plane we cut the mesh with for that 
        Vector3 up = new Vector3()
        {
            x = plane.normal.x,
            y = plane.normal.y,
            z = plane.normal.z
        };

        Vector3 left = Vector3.Cross(plane.normal, up);

        Vector3 displacement = Vector3.zero;
        Vector2 uv1 = Vector2.zero;
        Vector2 uv2 = Vector2.zero;

        for (int i = 0; i < vertices.Count; i++) {
            displacement = vertices[i] - centerPosition;
            uv1 = new Vector2()
            {
                x = .5f + Vector3.Dot(displacement, left),
                y = .5f + Vector3.Dot(displacement, up)
            };

            displacement = vertices[(i + 1) % vertices.Count] - centerPosition;
            uv2 = new Vector2()
            { 
                x = .5f + Vector3.Dot(displacement, left),
                y = .5f + Vector3.Dot(displacement, up)
            };

            Vector3[] newVertices = {vertices[i], vertices[(i+1) % vertices.Count], centerPosition};
			Vector3[] normals = {-plane.normal, -plane.normal, -plane.normal};
			Vector2[] uvs   = {uv1, uv2, new(0.5f, 0.5f)};

            MeshTriangle currentTriangle = new MeshTriangle(newVertices, normals, uvs, originalMesh.subMeshCount + 1);

            if(Vector3.Dot(Vector3.Cross(newVertices[1] - newVertices[0],newVertices[2] - newVertices[0]),normals[0]) < 0)
                FlipTriangle(currentTriangle);

            leftMesh.AddTriangle(currentTriangle);

            normals = new[] { plane.normal, plane.normal, plane.normal };
            currentTriangle = new MeshTriangle(newVertices, normals, uvs, originalMesh.subMeshCount + 1);

            if(Vector3.Dot(Vector3.Cross(newVertices[1] - newVertices[0],newVertices[2] - newVertices[0]),normals[0]) < 0)
                FlipTriangle(currentTriangle);

            rightMesh.AddTriangle(currentTriangle);
        
        }
    }
}
*/