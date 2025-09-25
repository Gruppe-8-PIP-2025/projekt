using UnityEngine;

/// <author>
/// Can Özbal (canoezbal@gmail1.com)
/// </author>
/// <summary>
/// Stores data for collectible or usable resources in the game, such as ores,
/// plants, or other gatherable materials. 
/// Inherits from <see cref="GameEntityData"/>.
/// </summary>
[CreateAssetMenu(fileName = "NewResource", menuName = "Game Entities/Resource")]
public class ResourceData : GameEntityData
{
    [Header("Resource Properties")]

    /// <summary>
    /// The display name of the resource.
    /// </summary>
    public string resourceName;

    /// <summary>
    /// The maximum amount of this resource that can be collected or stored.
    /// </summary>
    public int maxAmount;

    /// <summary>
    /// The rate at which this resource regenerates over time.
    /// </summary>
    public float regenerationRate;
}
