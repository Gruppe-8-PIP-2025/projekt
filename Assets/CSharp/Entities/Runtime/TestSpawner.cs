using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    [SerializeField] private EntityManager entityManager

    private void Start()
    {
        if (entityManager == null)
        {
            Debug.LogError("EntityManager reference not set in TestSpawner!");
            return;
        }

        entityManager.SpawnEntity("Smelter", new Vector3(0, 0, 0));
    }
}
