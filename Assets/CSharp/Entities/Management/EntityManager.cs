using UnityEngine;

/// <author>
/// Can Özbal (canoezbal@gmail1.com)
/// </author>
/// <summary>
/// Manages spawning and references for entities in the scene.
/// </summary>
public class EntityManager : MonoBehaviour
{
    [SerializeField] private GameObject smelterPrefab;
    [SerializeField] private string buildingType = "Smelter";

    /// <summary>
    /// Internal spawn method. Only called by wrapper methods.
    /// </summary>
    /// <param name="entityName">Name of the entity to spawn.</param>
    /// <param name="position">World position to spawn at.</param>
    private void SpawnEntityInternal(string entityName, Vector3 position)
    {
        if (entityName == "Smelter" && smelterPrefab != null)
            Instantiate(smelterPrefab, position, Quaternion.identity);
        else
            Debug.LogWarning($"Cannot spawn entity: {entityName}");
    }

    /// <summary>
    /// Spawns a Smelter entity at the given position.
    /// </summary>
    /// <param name="position">World position to spawn the Smelter.</param>
    public void SpawnSmelter(Vector3 position)
    {
        SpawnEntityInternal("Smelter", position);
    }

    /// <summary>
    /// Returns the type of building this manager spawns.
    /// </summary>
    public string BuildingType => buildingType;
}
