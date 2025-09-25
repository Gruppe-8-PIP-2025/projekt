using UnityEngine;

/// <author>
/// Can Özbal (canoezbal@gmail1.com)
/// </author>
/// <summary>
/// Stores data for terrain obstacles in the game, such as rocks or trees.
/// Inherits from <see cref="GameEntityData"/>.
/// </summary>
[CreateAssetMenu(fileName = "NewTerrainObstacle", menuName = "Game Entities/TerrainObstacle")]
public class TerrainObstacleData : GameEntityData
{
    [Header("Terrain/Obstacle Properties")]

    /// <summary>
    /// Whether this obstacle can be destroyed by the player or other systems.
    /// </summary>
    public bool isDestructible;

    /// <summary>
    /// The health of the obstacle. Only relevant if <see cref="isDestructible"/> is true.
    /// </summary>
    public float health;
}

