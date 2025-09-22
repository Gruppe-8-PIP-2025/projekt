using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework.Internal;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class GridManager : MonoBehaviour
{
  [SerializeField] private GridDimensions gridDimensions;
  [SerializeField] private CursorUtility cursorUtility;
  [SerializeField] private GameObject gridTilePrefab;

  #region Test/Debug
  [SerializeField] private GameObject testCube;
  public void OnClickPlaceTestCube(CallbackContext ctx)
  {
    GridTile position = GetTileAtCursor();

    if (position == null)
      return;

    IPlaceable placeable = testCube.GetComponent<TestPlaceable>();
    RemovePlaceable(placeable);
    AddPlaceable(placeable,position);
  }
  private void TestAwake()
  {
    AddPlaceable(testCube.GetComponent<TestPlaceable>(), GetTileByGridCoordinates(new Vector2(4,10)));
  }
  #endregion

  // describes the size and position of the grid in unity's scene coordinates 
  public Rectangle Boundary { get; private set; }

  // describes the size of the grid in tiles
  public Vector2 Dimensions { get; private set; }

  // a list of all tiles within the grid
  public List<GridTile> GridTiles { get; private set; }

  public GridTile GetTileAtCursor()
  {
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    // Casts the ray and get the first game object hit 
    if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("GridTile")))
    {
      return hit.transform.gameObject.GetComponentInParent<GridTile>();
    }
    else
    {
      return null;
    }
  }

  // returns false if the IPlaceable overlaps with any conflicting IPlaceables already in the grid
  // returns true in all other cases
  public bool ValidatePlacement(IPlaceable placeable, Vector2 position) =>
    ValidatePlacement(placeable, GetTileByGridCoordinates(position));

  public bool ValidatePlacement(IPlaceable placeable, GridTile position)
  {
    if (placeable == null)
      throw new NullReferenceException("IPlaceable is null.");
    if (position == null)
      throw new NullReferenceException($"GridTile is null. Total GridTiles: {GridTiles.Count}");

    return !(position.Contains.Any(p => p.Category == placeable.Category)
       || position.Intersects.Any(p => p.Category == placeable.Category));
  }

  // returns which tile has the given position in the grid
  // returns null if the position doesn't exist
  public GridTile GetTileByGridCoordinates(Vector2 value)
  {
    return GridTiles.Find(tile => tile.Position.Equals(value));
  }
  
  // puts the IPlaceable into the GridTile at the given grid coordinates/tile
  // This method should *not* implement ValidatePlacement and *can* place IPlaceables in invalid positions
  // returns void
  public void AddPlaceable(IPlaceable placeable, Vector2 position) =>
    AddPlaceable(placeable, GetTileByGridCoordinates(position));

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

    placeable.Transform.position = position.transform.position;
  }

  // removes the given IPlaceable from the GridTile that contains it
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
  
  // removes all IPlaceables from the given grid coordinates/tile
  // returns void
  public void PurgeTile(Vector2 position) =>
    PurgeTile(GetTileByGridCoordinates(position));

  public void PurgeTile(GridTile position)
  {
    foreach (IPlaceable placeable in position.Contains)
    {
      RemovePlaceable(placeable);
    }
  }
  
  // removes all IPlaceables of the given type from the given grid coordinates/tile
  public void PurgeTile(Vector2 position,Placeables placeableType) =>
    PurgeTile(GetTileByGridCoordinates(position),placeableType);
    
  public void PurgeTile(GridTile position, Placeables placeableType)
  {
    foreach (IPlaceable placeable in position.Contains.Where(p=>p.Category.Equals(placeableType)))
    {
      RemovePlaceable(placeable);
    }
  }

  #region MonoBehavior
  /// <summary>
  /// Awake is called once before the first execution of Update and Start after
  /// the MonoBehaviour is created. It will be called even if this GameObject is
  /// not active. 
  /// </summary>
  void Awake()
  {
    Boundary = gridDimensions.Value;
    Dimensions = gridDimensions.GridSize;

    GridTiles = new();
    for (int x = 1; x <= Dimensions.x; x++)
    {
      for (int y = 1; y <= Dimensions.y; y++)
      {
        GameObject gridTile = Instantiate(gridTilePrefab);

        gridTile.name = $"GridTile ({x},{y})";

        gridTile.transform.SetParent(gameObject.transform);

        GridTile gridTileComponent = gridTile.GetComponent<GridTile>();

        gridTileComponent.SetScale(gridDimensions.TileDimensions);

        gridTileComponent.SetScenePosition(new Vector2(
          10 * (Boundary.Left + (x-1) * gridDimensions.TileDimensions.x),
          10 * (Boundary.Top - (y-1) * gridDimensions.TileDimensions.y)
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
}