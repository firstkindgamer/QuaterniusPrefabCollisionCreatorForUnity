using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

class ApplyCollisionPrefabs
{
    [MenuItem("Tools/Apply Collision Prefabs to Selected Quaternius Models")]
    public static void CombineCollisions()
    {
        Debug.Log("CombineCollisions called");
        ApplyCollisionPrefabs applyCollisionPrefabs = new();
        applyCollisionPrefabs.ApplyCollisionPrefabsToAllImportedModels();
    }

    public void ApplyCollisionPrefabsToModel(GameObject modelInstance, GameObject collisionInstance)
    {
        MeshRenderer[] meshRenderers = modelInstance.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in meshRenderers)
        {

            if (renderer.gameObject.GetComponent<Collider>() != null)
            {
                renderer.enabled = false;
                continue;

            }
            renderer.gameObject.AddComponent<MeshCollider>();
            renderer.enabled = false;
        }
        

        collisionInstance.transform.parent = modelInstance.transform;
        collisionInstance.transform.localPosition = Vector3.zero;
        collisionInstance.transform.localRotation = Quaternion.identity;
        collisionInstance.transform.localScale = Vector3.one;


    }
    public void ApplyCollisionPrefabsToAllImportedModels()
    {


        string[] importedModelPaths = Selection.GetFiltered<UnityEngine.Object>(SelectionMode.Assets)
            .Where(obj => AssetDatabase.GetMainAssetTypeAtPath(AssetDatabase.GetAssetPath(obj)) == typeof(GameObject))
            .Select(obj => AssetDatabase.GetAssetPath(obj))
            .ToArray();
        foreach (string modelPath in importedModelPaths)
        {
            GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(modelPath);
            if (model != null)
            {
                GameObject modelInstance = Object.Instantiate(model);
                modelInstance.name = model.name;
                string prefabPath = Path.Join("Assets", "Prefabs", model.name + "_WithCollisions.prefab");
                string collisionpath = Path.Join(Path.GetDirectoryName(modelPath), "Collisions", "Collision_" + modelPath.Split(Path.AltDirectorySeparatorChar).Last());
                GameObject collisionPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(collisionpath);
                if (collisionPrefab == null)
                {
                    Debug.LogWarning("No collision prefab found for model: " + model.name + " at path: " + collisionpath + "\n"
                    + "Creating prefab without collisions.");
                    PrefabUtility.SaveAsPrefabAsset(modelInstance, prefabPath);
                    GameObject.DestroyImmediate(modelInstance);
                    continue;
                }
                GameObject collisionInstance = Object.Instantiate(collisionPrefab);


                // Apply collision prefabs to the model instance
                ApplyCollisionPrefabsToModel(modelInstance, collisionInstance);

                // Save the modified model instance as a new prefab
                
                PrefabUtility.SaveAsPrefabAsset(modelInstance, prefabPath);

                // Clean up the instantiated model instance
                GameObject.DestroyImmediate(modelInstance);

                Debug.Log("Processed model: " + model.name);
            }
            else
            {
                Debug.LogError("Failed to load model at path: " + modelPath);
            }
        }
    }
}
