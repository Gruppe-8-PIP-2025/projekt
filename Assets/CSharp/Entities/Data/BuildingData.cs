using UnityEngine;

/// <author>
/// Can Özbal (canoezbal@gmail1.com)
/// </author>
/// <summary>
/// Stores data specific to buildings in the game.
/// Inherits from GameEntityData.
/// </summary>
[CreateAssetMenu(fileName = "NewBuilding", menuName = "Game Entities/Building")]
public class BuildingData : GameEntityData
{
    [Header("Building Properties")]

    /// <summary>
    /// Type of the building (Factory, Refinery, etc.).
    /// </summary>
    [SerializeField] private BuildingTypes buildingType;

    /// <summary>
    /// Display name of the building (localized).
    /// </summary>
    public string localizedName;

    /// <summary>
    /// How fast this building crafts items.
    /// </summary>
    public float craftingSpeed;

    /// <summary>
    /// Size of the building in tiles (width, height).
    /// </summary>
    public Vector2 dimensions;

    /// <summary>
    /// Number of pipe connections the building has.
    /// </summary>
    public int pipeConnections;

    /// <summary>
    /// Whether the building allows a drone connection.
    /// </summary>
    public bool droneConnection;

    [Header("UI")]

    /// <summary>
    /// Icon used for the building in the UI menu.
    /// </summary>
    [SerializeField] private Texture menuIcon;

    /// <summary>
    /// Returns the menu icon.
    /// </summary>
    public Texture MenuIcon => menuIcon;

    /// <summary>
    /// Returns the building type.
    /// </summary>
    public BuildingTypes BuildingType => buildingType;
}
