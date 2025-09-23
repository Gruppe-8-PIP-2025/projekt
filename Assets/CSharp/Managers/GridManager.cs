using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

/// <author>Maria Wickes (maria.lindling@protonmail.com)</author>
/// <summary>
/// Class for controlling and organizing GridTiles and their contents.
/// </summary>
public class GridManager : MonoBehaviour
{
  #region Unity Editor Fields
  /// <summary>
  /// A collection of parameters that determine GridTile size and the dimensions
  /// of the grid itself.
  /// </summary>
  [SerializeField] private GridParameters gridParams;
  #endregion


  #region Properties
  /// <summary>
  /// Describes the size and position of the grid in unity's scene coordinates.
  /// </summary>
  /// <remarks><b>WARNING:</b> <i>Y here is Z in Unity!</i></remarks>
  public RectangleF Boundary { get; private set; }

  /// <summary>
  /// Describes the size of the grid in tiles.
  /// </summary>
  /// <remarks><b>WARNING:</b> <i>Y here is Z in Unity!</i></remarks>
  public Vector2 Dimensions { get; private set; }

  /// <summary>
  /// A list containing all tiles within the grid.
  /// </summary>
  public List<GridTile> GridTiles { get; private set; }
  #endregion


  #region Validation
  /// <summary>
  /// Tests whether or not the given IPlaceable would conflict with any other
  /// objects of its category if placed in the GridTile at the given coordinates. 
  /// </summary>
  /// <param name="placeable">
  /// The IPlaceable being tested.
  /// </param>
  /// <param name="position">
  /// The X-Y position of the GridTile serving as the origin for the IPlaceable
  /// being tested.
  /// <br/><b>WARNING:</b> <i>Y here is Z in Unity!</i></param>
  /// <returns>
  /// Returns false if the IPlaceable overlaps with any conflicting
  /// IPlaceables already in the grid.
  /// <br/>Returns true in all other cases.
  /// </returns>
  /// <remarks>
  /// An IPlaceable is conflicting if it has the same Placeables value as its
  /// Category as the IPlaceable being validated for placement.
  /// </remarks>
  public bool ValidatePlacement(IPlaceable placeable, Vector2 position) =>
    ValidatePlacement(placeable, GetTileByGridCoordinates(position));

  /// <summary>
  /// Tests whether or not the given IPlaceable would conflict with any other
  /// objects of its category if placed in the given GridTile. 
  /// </summary>
  /// <param name="placeable">
  /// The IPlaceable being tested.
  /// </param>
  /// <param name="position">
  /// The GridTile serving as the origin for the IPlaceable being tested.
  /// </param>
  /// <returns>
  /// Returns false if the IPlaceable overlaps with any conflicting
  /// IPlaceables already in the grid.
  /// <br/>Returns true in all other cases.
  /// </returns>
  /// <exception cref="NullReferenceException">Any parameter is null.</exception>
  /// <remarks>
  /// An IPlaceable is conflicting if it has the same Placeables value as its
  /// Category as the IPlaceable being validated for placement.
  /// </remarks>
  public bool ValidatePlacement(IPlaceable placeable, GridTile position)
  {
    if (placeable == null)
      throw new NullReferenceException("IPlaceable is null.");
    if (position == null)
      throw new NullReferenceException($"GridTile is null. Total GridTiles: {GridTiles.Count}");

    List<Placeables> categories = new() { placeable.Category };

    for (int x = 0; x < placeable.Dimensions.x; x++)
    {
      for (int y = 0; y < placeable.Dimensions.y; y++)
      {
        if (GridTileContains(GetTileByGridCoordinates(new Vector2(position.Position.x + x, position.Position.y + y)), categories))
        { return false; }
      }
    }
    return true;
  }

  /// <summary>
  /// Tests whether or not the given IPlaceable would, if placed in the GridTile
  /// at the given coordinates, conflict with any objects that have any of the
  /// given categories.
  /// </summary>
  /// <param name="placeable">
  /// The IPlaceable being tested.
  /// </param>
  /// <param name="position">
  /// The GridTile serving as the origin for the IPlaceable being tested.
  /// </param>
  /// <param name="categories">
  /// The categories of Placeable that qualify an IPlaceable as conflicting.
  /// </param>
  /// <returns>
  /// Returns false if the IPlaceable overlaps with any conflicting
  /// IPlaceables already in the grid.
  /// <br/>Returns true in all other cases.
  /// </returns>
  public bool ValidatePlacement(IPlaceable placeable, Vector2 position, IEnumerable<Placeables> categories) =>
    ValidatePlacement(placeable, GetTileByGridCoordinates(position), categories);

  /// <summary>
  /// Tests whether or not the given IPlaceable would, if placed in the given
  /// GridTile, conflict with any objects that have any of the given categories.
  /// </summary>
  /// <param name="placeable">
  /// The IPlaceable being tested.
  /// </param>
  /// <param name="position">
  /// The GridTile serving as the origin for the IPlaceable being tested.
  /// </param>
  /// <param name="categories">
  /// The categories of Placeable that qualify an IPlaceable as conflicting.
  /// </param>
  /// <returns>
  /// Returns false if the IPlaceable overlaps with any conflicting
  /// IPlaceables already in the grid.
  /// <br/>Returns true in all other cases.
  /// </returns>
  /// <exception cref="NullReferenceException">
  /// If placeable or position parameter is null.
  /// </exception>
  public bool ValidatePlacement(IPlaceable placeable, GridTile position, IEnumerable<Placeables> categories)
  {
    if (placeable == null)
      throw new NullReferenceException("IPlaceable is null.");
    if (position == null)
      throw new NullReferenceException($"GridTile is null. Total GridTiles: {GridTiles.Count}");

    for (int x = 0; x < placeable.Dimensions.x; x++)
    {
      for (int y = 0; y < placeable.Dimensions.y; y++)
      {
        if (GridTileContains(GetTileByGridCoordinates(new Vector2(position.Position.x + x, position.Position.y + y)), categories))
        { return false; }
      }
    }
    return true;
  }

  /// <summary>
  /// Checks if the given GridTile contains or intersects with any IPlaceables
  /// with the given category.
  /// </summary>
  /// <param name="position">The GridTile being tested.</param>
  /// <param name="categories">A list of categories to check for.</param>
  /// <returns>Returns true if the GridTile contains or intersects with an
  /// IPlaceable of the given categories.
  /// <br/>Returns false in all other cases.</returns>
  private bool GridTileContains(GridTile position, IEnumerable<Placeables> categories) =>
    position.Contains.Any(p => categories.Contains(p.Category))
    || position.Intersects.Any(p => categories.Contains(p.Category));
  #endregion


  #region GridTile Selection
  /// <summary>
  /// Gets the GridTile at the cursor's current position relative to the active
  /// MainCamera.
  /// </summary>
  /// <returns>
  /// Returns the GridTile at the current cursor position.
  /// <br/>Returns null if no GridTile is found.
  /// </returns>
  public GridTile GetTileAtCursor()
  {
    if (
      Physics.Raycast(
        Camera.main.ScreenPointToRay(Input.mousePosition),
        out RaycastHit hit,
        Mathf.Infinity,
        LayerMask.GetMask("GridTile")
      ))
    {
      return hit.transform.gameObject.GetComponentInParent<GridTile>();
    }
    else
    {
      Debug.LogWarning("Raycast for GetTileAtCursor failed to hit a GridTile.");
      return null;
    }
  }

  /// <summary>
  /// Gets the GridTile that's located at the given coordinates within the grid. 
  /// </summary>
  /// <param name="value">The coordinates of the wanted GridTile.</param>
  /// <returns>
  /// Returns which tile has the given position in the grid.
  /// <br/>Returns null if the position doesn't exist.
  /// </returns>
  /// <remarks><b>WARNING:</b> <i>Y here is Z in Unity!</i></remarks>
  public GridTile GetTileByGridCoordinates(Vector2 value)
  {
    return GridTiles.Find(tile => tile.Position.Equals(value));
  }
  #endregion


  #region Placeable/Tile Control
  /// <summary>
  /// Puts the given IPlaceable into the GridTile at the given grid coordinates.
  /// </summary>
  /// <param name="placeable">The IPlaceable to be placed.</param>
  /// <param name="position">
  /// The coordinates of the GridTile in which to place the IPlaceable.
  /// </param>
  /// <remarks>
  /// This method should *not* implement ValidatePlacement itself and can place
  /// IPlaceables in invalid positions.
  /// <br/><b>WARNING:</b> <i>Y here is Z in Unity!</i>
  /// </remarks>
  public void AddPlaceable(IPlaceable placeable, Vector2 position) =>
    AddPlaceable(placeable, GetTileByGridCoordinates(position));


  /// <summary>
  /// Puts the given IPlaceable into the given GridTile.
  /// </summary>
  /// <param name="placeable">The IPlaceable to be placed.</param>
  /// <param name="position">The GridTile in which to place the IPlaceable.</param>
  /// <exception cref="NullReferenceException">Any parameter is null.</exception>
  /// <remarks>
  /// This method should *not* implement ValidatePlacement itself and can place
  /// IPlaceables in invalid positions.
  /// </remarks>
  public void AddPlaceable(IPlaceable placeable, GridTile position)
  {
    if (placeable == null)
      throw new NullReferenceException("IPlaceable is null.");
    if (position == null)
      throw new NullReferenceException($"GridTile is null. Total GridTiles: {GridTiles.Count}");

    for (
      int x = (int)position.Position.x;
      x <= position.Position.x + placeable.Dimensions.x;
      x++)
    {
      for (
        int y = (int)position.Position.y;
        y <= position.Position.y + placeable.Dimensions.y;
        y++)
      {
        GridTile gridTile = GetTileByGridCoordinates(new Vector2(x, y));

        if (gridTile == null)
          return;

        gridTile.AddIntersect(placeable);
      }
    }

    position.AddPlaceable(placeable);

    // TODO: Move this code-section to GridTile
    placeable.Transform.position = position.transform.position;
    // --
  }

  /// <summary>
  /// Removes the given IPlaceable from the GridTile that contains it.
  /// </summary>
  /// <param name="placeable">The IPlaceable being removed.</param>
  /// <exception cref="NullReferenceException">Any parameter is null.</exception>
  /// <remarks>Currently does not Destroy the IPlaceable.</remarks>
  public void RemovePlaceable(IPlaceable placeable)
  {
    if (placeable == null)
      throw new NullReferenceException("IPlaceable is null.");

    foreach (GridTile gridTile in GridTiles)
    {
      gridTile.RemovePlaceable(placeable);
      gridTile.RemoveIntersect(placeable);
    }
  }

  /// <summary>
  /// Removes all IPlaceables from the GridTile at the given coordinates.
  /// </summary>
  /// <param name="position">The coordinates the GridTile to be purged.</param>
  /// <remarks><b>WARNING:</b> <i>Y here is Z in Unity!</i></remarks>
  public void PurgeTile(Vector2 position) =>
    PurgeTile(GetTileByGridCoordinates(position));

  /// <summary>
  /// Removes all IPlaceables from the given GridTile.
  /// </summary>
  /// <param name="position">The GridTile to be purged.</param>
  public void PurgeTile(GridTile position)
  {
    foreach (IPlaceable placeable in position.Contains)
    {
      RemovePlaceable(placeable);
    }
  }

  /// <summary>
  /// Removes all IPlaceables of the given Placeables type from the GridTile at
  /// the given coordinates.
  /// </summary>
  /// <param name="position">The coordinates the GridTile to be purged.</param>
  /// <param name="placeableType">The type of Placeables to purge.</param>
  /// <remarks><b>WARNING:</b> <i>Y here is Z in Unity!</i></remarks>
  public void PurgeTile(Vector2 position, Placeables placeableType) =>
    PurgeTile(GetTileByGridCoordinates(position), placeableType);

  /// <summary>
  /// Removes all IPlaceables of the given Placeables type from the GridTile at
  /// the given coordinates.
  /// </summary>
  /// <param name="position">The GridTile to be purged.</param>
  /// <param name="placeableType">The type of Placeables to purge.</param>
  public void PurgeTile(GridTile position, Placeables placeableType)
  {
    foreach (IPlaceable placeable in position.Contains.Where(p => p.Category.Equals(placeableType)))
    {
      RemovePlaceable(placeable);
    }
  }
  #endregion


  #region MonoBehavior
  /// <summary>
  /// Awake is called once before the first execution of Update and Start after
  /// the MonoBehaviour is created. It will be called even if this GameObject is
  /// not active. 
  /// </summary>
  void Awake()
  {
    Boundary = gridParams.SceneBoundary;
    Dimensions = gridParams.Dimensions;

    GridTiles = new();
    for (int x = 1; x <= Dimensions.x; x++)
    {
      for (int y = 1; y <= Dimensions.y; y++)
      {
        GameObject gridTile = Instantiate(gridParams.GridTilePrefab);

        gridTile.name = $"GridTile ({x},{y})";

        gridTile.transform.SetParent(gameObject.transform);

        GridTile gridTileComponent = gridTile.GetComponent<GridTile>();

        gridTileComponent.SetScale(gridParams.TileScale);

        gridTileComponent.SetScenePosition(new Vector2(
          10 * (Boundary.Left + (x - 1) * gridParams.TileScale.x),
          10 * (Boundary.Top - (y - 1) * gridParams.TileScale.y)
        ));

        gridTileComponent.SetPosition(new Vector2(x, y));

        GridTiles.Add(gridTileComponent);
      }
    }

    TestAwake();
  }

  /// <summary>
  /// Start is called once before the first execution of Update after the
  /// MonoBehaviour is created.
  /// </summary>
  void Start()
  {
  }

  /// <summary>
  /// Update is called once every frame.
  /// </summary>
  void Update()
  {
  }
  #endregion


  #region Test/Debug
  [SerializeField] private GameObject testCube;
  public void OnClickPlaceTestCube(CallbackContext ctx)
  {
    GridTile position = GetTileAtCursor();

    if (position == null)
      return;

    IPlaceable placeable = testCube.GetComponent<TestPlaceable>();
    RemovePlaceable(placeable);
    AddPlaceable(placeable, position);
  }
  private void TestAwake()
  {
    AddPlaceable(testCube.GetComponent<TestPlaceable>(), GetTileByGridCoordinates(new Vector2(4, 10)));
  }
  #endregion
}