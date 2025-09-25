using UnityEngine;

/// <author>
/// Can Özbal (canoezbal@gmail1.com)
/// </author>
/// <summary>
/// Utility for spawning entities directly from GameEntityData ScriptableObjects.
/// </summary>
public class EntitySpawner : MonoBehaviour
{
    /// <summary>
    /// Instantiates an entity based on the provided GameEntityData.
    /// </summary>
    /// <param name="data">The GameEntityData describing the entity to spawn.</param>
    /// <param name="position">World position where the entity will be spawned.</param>
    /// <param name="parent">Optional transform to set as the parent of the spawned entity.</param>
    /// <returns>The instantiated GameObject, or null if the asset was missing.</returns>
    public static GameObject SpawnEntity(GameEntityData data, Vector3 position, Transform parent = null)
    {
        if (data.asset == null)
        {
            Debug.LogWarning($"Entity {data.name} has no asset assigned.");
            return null;
        }

        GameObject obj = Instantiate(data.asset, position + data.assetOffset, data.rotation, parent);
        obj.transform.localScale = data.scale;

        var entityTag = obj.AddComponent<EntityReference>();
        entityTag.data = data;

        return obj;
    }
}
