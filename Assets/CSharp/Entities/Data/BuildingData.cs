using UnityEngine;

[CreateAssetMenu(fileName = "NewBuilding", menuName = "Game Entities/Building")]
public class BuildingData : GameEntityData
{
    [Header("Building Properties")]
    public string localizedName;
    public BuildingTypes buildingType;
    public float craftingSpeed;
    public Vector2 dimensions;
    public int pipeConnections;
    public bool droneConnection;
}
