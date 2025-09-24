using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    void Start()
    {
        // Beispiel: spawnt ein "Factory" Building bei (0,0,0)
        EntityManager.Instance.SpawnEntity("Smelter", Vector3.zero);

        // Beispiel: spawnt ein "Tree" Obstacle bei (5,0,0)
        EntityManager.Instance.SpawnEntity("Tree", new Vector3(5, 0, 0));

        // Beispiel: spawnt ein "IronOre" Resource bei (10,0,0)
        EntityManager.Instance.SpawnEntity("IronOre", new Vector3(10, 0, 0));
    }
}
