using System.Drawing;
using UnityEngine;

/// <author>Maria Wickes (maria.lindling@protonmail.com)</author>
/// <summary>
/// Serializable object used to carry a collection of parameters for GridManager.
/// </summary>
[System.Serializable]
public class GridParameters
{
  #region Constants
  private const float GRIDTILEBASESIZE = 10.0f;
  #endregion


  #region Unity Editor Fields
  /// <summary>
  /// Describes the dimensions of GridTiles in Scene coordinates.
  /// <br/>A scale of 1.0f / 1.0f is ten by ten units in Unity.
  /// </summary>
  /// <remarks><b>WARNING:</b> <i>Y here is Z in Unity!</i></remarks>
  [SerializeField] private Vector2 tileScale;
  
  /// <summary>
  /// Prefab GameObject from which GridTile objects will be created.
  /// </summary>
  /// <remarks>
  /// Structure must be:
  /// <br/>Parent -&gt; GameObject (Type: Empty, Component: GridTile)
  /// <br/>Child -&gt; GameObject (Type: Plane)
  /// </remarks>
  [SerializeField] private GameObject gridTilePrefab;

  /// <summary>
  /// Describes the size of the grid in GridTiles.
  /// </summary>
  /// <remarks><b>WARNING:</b> <i>Y here is Z in Unity!</i></remarks>
  [SerializeField] private Vector2 gridDimensions;
  #endregion
  
  
  #region Properties: GridTile
  /// <summary>
  /// Describes the dimensions of GridTiles in Scene coordinates.
  /// <br/>A scale of 1.0f / 1.0f is ten by ten units in Unity.
  /// </summary>
  /// <remarks><b>WARNING:</b> <i>Y here is Z in Unity!</i></remarks>
  public Vector2 TileScale => tileScale;

  /// <summary>
  /// Prefab GameObject from which GridTile objects will be created.
  /// </summary>
  public GameObject GridTilePrefab => gridTilePrefab;
  #endregion

  #region Properties: Grid
  /// <summary>
  /// Describes the dimensions of the grid in GridTiles.
  /// </summary>
  /// <remarks><b>WARNING:</b> <i>Y here is Z in Unity!</i></remarks>
  public Vector2 Dimensions => gridDimensions;

  /// <summary>
  /// Describes the dimensions of the grid in the Unity Scene. 
  /// </summary>
  /// <remarks><b>WARNING:</b> <i>Y here is Z in Unity!</i></remarks>
  private Vector2 SceneSize => new(
    GRIDTILEBASESIZE * tileScale.x * gridDimensions.x,
    GRIDTILEBASESIZE * tileScale.y * gridDimensions.y
  );

  /// <summary>
  /// Describes the boundaries of the grid in the Unity Scene.
  /// </summary>
  /// <remarks><b>WARNING:</b> <i>Y here is Z in Unity!</i></remarks>
  public RectangleF SceneBoundary => new(
    -1 * (tileScale.x * gridDimensions.x / 2),
    -1 * (tileScale.y * gridDimensions.y / 2),
    SceneSize.x,
    SceneSize.y
  );
  #endregion
}