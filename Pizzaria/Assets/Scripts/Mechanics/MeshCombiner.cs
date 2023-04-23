using UnityEngine;
using System.Collections;
 
public class MeshCombiner: MonoBehaviour{
    public static void Combine(GameObject parentObject)
    {
        ArrayList materials = new ArrayList();
        ArrayList combineInstanceArrays = new ArrayList();
        Matrix4x4 myTransform = parentObject.transform.worldToLocalMatrix;
        MeshFilter[] meshFilters = parentObject.GetComponentsInChildren<MeshFilter> ();
        foreach (MeshFilter meshFilter in meshFilters) {
            MeshRenderer meshRenderer = meshFilter.GetComponent<MeshRenderer> ();
            // Handle bad input
            if (!meshRenderer) { 
                Debug.LogError ("MeshFilter does not have a coresponding MeshRenderer.");
                continue; 
            }
            if (meshRenderer.materials.Length != meshFilter.sharedMesh.subMeshCount) {
                Debug.LogError ("Mismatch between material count and submesh count. Is this the correct MeshRenderer?"); 
                continue; 
            }
            for (int s = 0; s < meshFilter.sharedMesh.subMeshCount; s++) {
                int materialArrayIndex = 0;
                for (materialArrayIndex = 0; materialArrayIndex < materials.Count; materialArrayIndex++) {
                    if (materials [materialArrayIndex] == meshRenderer.sharedMaterials [s])
                        break;
                }
    
                if (materialArrayIndex == materials.Count) {
                    materials.Add (meshRenderer.sharedMaterials [s]);
                    combineInstanceArrays.Add (new ArrayList ());
                }
    
                CombineInstance combineInstance = new CombineInstance ();
                combineInstance.transform = myTransform * meshRenderer.transform.localToWorldMatrix;
                combineInstance.subMeshIndex = s;
                combineInstance.mesh = meshFilter.sharedMesh;
                (combineInstanceArrays [materialArrayIndex] as ArrayList).Add (combineInstance);
            }
        }
 
        // Get / Create mesh filter
         MeshFilter meshFilterCombine = parentObject.GetComponent<MeshFilter> ();
         if (!meshFilterCombine)
             meshFilterCombine = parentObject.AddComponent<MeshFilter> ();
 
         // Combine by material index into per-material meshes
         // also, Create CombineInstance array for next step
         Mesh[] meshes = new Mesh[materials.Count];
         CombineInstance[] combineInstances = new CombineInstance[materials.Count];
 
         for (int m = 0; m < materials.Count; m++) {
             CombineInstance[] combineInstanceArray = (combineInstanceArrays [m] as ArrayList).ToArray (typeof(CombineInstance)) as CombineInstance[];
             meshes [m] = new Mesh ();
             meshes [m].CombineMeshes (combineInstanceArray, true, true);
             combineInstances [m] = new CombineInstance ();
             combineInstances [m].mesh = meshes [m];
             combineInstances [m].subMeshIndex = 0;
         }
 
         // Combine into one
         meshFilterCombine.sharedMesh = new Mesh ();
         meshFilterCombine.sharedMesh.CombineMeshes (combineInstances, false, false);
 
         // Destroy other meshes
         foreach (Mesh mesh in meshes) {
             mesh.Clear ();
             DestroyImmediate (mesh);
         }

        // Get / Create mesh renderer
         MeshRenderer meshRendererCombine = parentObject.GetComponent<MeshRenderer> ();
         if (!meshRendererCombine)
             meshRendererCombine = parentObject.AddComponent<MeshRenderer> ();    
 
         // Assign materials
         Material[] materialsArray = materials.ToArray (typeof(Material)) as Material[];
         meshRendererCombine.materials = materialsArray;
    }

    private static int Contains(ArrayList searchList, string searchName)
    {
        for (int i = 0; i < searchList.Count; i++)
        {
            if (((Material)searchList[i]).name == searchName)
            {
                return i;
            }
        }
        return -1;
    }
}