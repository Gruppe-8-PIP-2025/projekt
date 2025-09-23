using UnityEngine;


public abstract class GameEntityData : ScriptableObject
{
    [Header("Shared Properties")]
    public GameObject asset;            
    public Vector3 assetOffset = Vector3.zero;
    public Quaternion rotation = Quaternion.identity;
    public Vector3 scale = Vector3.one;
}


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

public enum BuildingTypes
{
    Factory,
    Refinery,
    PowerPlant,
    Storage,
    Custom
}


[CreateAssetMenu(fileName = "NewTerrainObstacle", menuName = "Game Entities/TerrainObstacle")]
public class TerrainObstacleData : GameEntityData
{
    [Header("Terrain/Obstacle Properties")]
    public bool isDestructible;
    public float health;
}


[CreateAssetMenu(fileName = "NewResource", menuName = "Game Entities/Resource")]
public class ResourceData : GameEntityData
{
    [Header("Resource Properties")]
    public string resourceName;
    public int maxAmount;
    public float regenerationRate; 
}


[CreateAssetMenu(fileName = "NewObjective", menuName = "Game Entities/Objective")]
public class ObjectiveData : GameEntityData
{
    [Header("Objective Properties")]
    public string description;
    public bool isPrimaryObjective;
    public int requiredAmount;
}
