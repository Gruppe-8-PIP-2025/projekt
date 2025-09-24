using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    public static GameObject SpawnEntity(GameEntityData data, Vector3 position, Transform parent = null)
    {
        if (data.asset == null)
        {
            Debug.LogWarning($"Entity {data.name} has no asset assigned.");
            return null;
        }

        // Instantiate prefab
        GameObject obj = Instantiate(data.asset, position + data.assetOffset, data.rotation, parent);

        // Apply scaling
        obj.transform.localScale = data.scale;

        // Tag object with ScriptableObject reference (optional)
        var entityTag = obj.AddComponent<EntityReference>();
        entityTag.data = data;

        return obj;
    }
}