using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

/// <author>
/// Can Özbal (canoezbal@gmail1.com)
/// </author>
/// <summary>
/// Example script for spawning entities using an EntityManager reference.
/// </summary>
public class TestSpawner : MonoBehaviour
{
    /// <summary>
    /// Reference to the EntityManager used to spawn entities.
    /// </summary>
    [SerializeField] private EntityManager entityManager;

    /// <summary>
    /// Called on the first frame. Spawns a test Smelter entity.
    /// </summary>
    private void Start()
    {
        if (entityManager == null)
        {
            Debug.LogError("EntityManager reference not set in TestSpawner!");
            return;
        }

        entityManager.SpawnSmelter(Vector3.zero);
    }
}
