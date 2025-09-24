using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Trying to spawn Smelter...");
        var obj = EntityManager.Instance.SpawnEntity("Smelter", Vector3.zero);

        if (obj == null)
            Debug.LogWarning("Spawn failed!");
        else
            Debug.Log("Spawn successful!");
    }
}
