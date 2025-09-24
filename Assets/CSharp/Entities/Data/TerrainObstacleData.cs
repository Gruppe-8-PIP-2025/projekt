using UnityEngine;

[CreateAssetMenu(fileName = "NewTerrainObstacle", menuName = "Game Entities/TerrainObstacle")]
public class TerrainObstacleData : GameEntityData
{
    [Header("Terrain/Obstacle Properties")]
    public bool isDestructible;
    public float health;
}
