using UnityEngine;
using System.Collections.Generic;

public class EntityManager : MonoBehaviour
{
    public static EntityManager Instance { get; private set; }

    private Dictionary<string, GameEntityData> entities = new Dictionary<string, GameEntityData>();


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // optional
        LoadAllEntities();
    }
    private void LoadAllEntities()
    {
        GameEntityData[] loadedEntities = Resources.LoadAll<GameEntityData>("Entities");
        foreach (var entity in loadedEntities)
        {
            if (!entities.ContainsKey(entity.name))
                entities.Add(entity.name, entity);
        }

        Debug.Log($"Loaded {entities.Count} entities from Resources/Entities/");
    }

    public GameEntityData GetEntity(string entityName)
    {
        entities.TryGetValue(entityName, out GameEntityData entity);
        return entity;
    }

    public GameObject SpawnEntity(string entityName, Vector3 position, Transform parent = null)
    {
        var entityData = GetEntity(entityName);
        if (entityData != null)
            return EntitySpawner.SpawnEntity(entityData, position, parent);

        Debug.LogWarning($"Entity {entityName} not found.");
        return null;
    }
}
