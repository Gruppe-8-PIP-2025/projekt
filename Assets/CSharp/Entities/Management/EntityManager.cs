using UnityEngine;
using System.Collections.Generic;

public class EntityManager : MonoBehaviour
{
    [SerializeField] private string resourcesPath = "Entities"; // Pfad in Resources
    private Dictionary<string, GameEntityData> entities = new Dictionary<string, GameEntityData>();


    private void LoadAllEntities()
    {
        GameEntityData[] loadedEntities = Resources.LoadAll<GameEntityData>(resourcesPath);

        foreach (var entity in loadedEntities)
        {
            if (!entities.ContainsKey(entity.name))
                entities.Add(entity.name, entity);
        }

        Debug.Log($"Loaded {entities.Count} entities from Resources/{resourcesPath}/");
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
