using UnityEngine;
using System.Collections.Generic;

public class EntityManager : MonoBehaviour
{
    [Header("Resources Path (relative to Resources/)")]
    [SerializeField] private string resourcesPath = "Entities";

    private Dictionary<string, GameObject> prefabLibrary = new Dictionary<string, GameObject>();

    private void Awake()
    {
        LoadAllPrefabs();
    }

    private void LoadAllPrefabs()
    {
        prefabLibrary.Clear();

        GameObject[] loadedPrefabs = Resources.LoadAll<GameObject>(resourcesPath);

        foreach (var prefab in loadedPrefabs)
        {
            if (!prefabLibrary.ContainsKey(prefab.name))
            {
                prefabLibrary.Add(prefab.name, prefab);
                Debug.Log($"[EntityManager] Loaded prefab: {prefab.name}");
            }
        }

        Debug.Log($"[EntityManager] ✅ Loaded {prefabLibrary.Count} prefabs from Resources/{resourcesPath}/");
    }

    public void SpawnEntity(string entityName, Vector3 position)
    {
        if (prefabLibrary.TryGetValue(entityName, out var prefab))
        {
            Instantiate(prefab, position, Quaternion.identity);
            Debug.Log($"[EntityManager] Spawned '{entityName}' at {position}");
        }
        else
        {
            Debug.LogError($"[EntityManager] ❌ Prefab '{entityName}' not found in library! " +
                           $"(Make sure it's in Resources/{resourcesPath}/)");
        }
    }
}
