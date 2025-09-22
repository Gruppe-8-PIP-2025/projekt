using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
  [SerializeField] private GridDimensions gridDimensions;
  [SerializeField] private GameObject testCube;

  // describes the size and position of the grid in unity's scene coordinates 
  public Rectangle Boundary { get; private set; }

  // describes the size of the grid in tiles
  public Vector2 Dimensions { get; private set; }

  // a list of all tiles within the grid
  public List<GridTile> GridTiles { get; private set; }

  // returns false if the IPlaceable overlaps with any conflicting IPlaceables already in the grid
  // returns true in all other cases
  public bool ValidatePlacement(IPlaceable placeable, Vector2 position) =>
    ValidatePlacement(placeable,GetTileByGridCoordinates(position));

  public bool ValidatePlacement(IPlaceable placeable, GridTile position) =>
    !(position.Contains.Any(p => p.Category == placeable.Category) ||
      position.Intersects.Any(p => p.Category == placeable.Category));

  // returns which tile corresponds with the given scene coordinates
  // returns null if the coordinates are not within the grid
  public GridTile GetTileBySceneCoordinates(Vector2 value)
  {
    return GetTileByGridCoordinates(new Vector2(
      (value.x - Boundary.Left) / gridDimensions.TileDimensions.x,
      (value.y - Boundary.Top) / gridDimensions.TileDimensions.y 
    ));
  }
  
  // returns which tile has the given position in the grid
  // returns null if the position doesn't exist
  public GridTile GetTileByGridCoordinates(Vector2 value) =>
    GridTiles.Find(tile=>tile.Position.Equals(value));

  // puts the IPlaceable into the GridTile at the given grid coordinates/tile
  // This method should *not* implement ValidatePlacement and *can* place IPlaceables in invalid positions
  // returns void
  public void AddPlaceable(IPlaceable placeable, Vector2 position) =>
    AddPlaceable(placeable,GetTileByGridCoordinates(position));

  public void AddPlaceable(IPlaceable placeable, GridTile position)
  {
    position.AddPlaceable(placeable);

    for (
      int x = (int)position.Position.x;
      x <= position.Position.x - placeable.Dimensions.x;
      x--)
    {
      for (
        int y = (int)position.Position.y;
        y <= position.Position.y - placeable.Dimensions.y;
        y--)
      {
        GetTileByGridCoordinates(new Vector2(x, y)).AddIntersect(placeable);
      }
    }
  }

  // removes the given IPlaceable from the GridTile that contains it
  public void RemovePlaceable(IPlaceable placeable)
  {
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
    GridTiles = new();
    Boundary = gridDimensions.Value;
    Dimensions = gridDimensions.GridSize;

    testCube.transform.position = new Vector3(Boundary.Top,0,Boundary.Left);
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