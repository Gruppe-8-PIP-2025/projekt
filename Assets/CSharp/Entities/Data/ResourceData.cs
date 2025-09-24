using UnityEngine;

[CreateAssetMenu(fileName = "NewResource", menuName = "Game Entities/Resource")]
public class ResourceData : GameEntityData
{
    [Header("Resource Properties")]
    public string resourceName;
    public int maxAmount;
    public float regenerationRate;
}
