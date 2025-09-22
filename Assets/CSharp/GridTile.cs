using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class GridTile : MonoBehaviour
{
  [SerializeField] private GameObject plane;

  /// <summary>
  /// Describes the tile's position in the grid.
  /// </summary>
  public Vector2 Position { get; private set; }

  public (
    Vector2 TopLeft, Vector2 Top, Vector2 TopRight,
    Vector2 Left, Vector2 Right,
    Vector2 BottomLeft, Vector2 Bottom, Vector2 BottomRight
    ) Adjacencies =>
    (
      Position + new Vector2(-1,  1),
      Position + new Vector2( 0,  1),
      Position + new Vector2( 1,  1),
      Position + new Vector2(-1,  0),
      Position + new Vector2( 1,  0),
      Position + new Vector2(-1, -1),
      Position + new Vector2( 0, -1),
      Position + new Vector2( 1, -1)
    );

  public List<Vector2> Neighbours => new()
  {
    Adjacencies.TopLeft,
    Adjacencies.Top,
    Adjacencies.TopRight,
    Adjacencies.Left,
    Adjacencies.Right,
    Adjacencies.BottomLeft,
    Adjacencies.Bottom,
    Adjacencies.BottomLeft
  };

  // describes the tile's size and position in unity's scene coordinates
  public Rectangle Boundary { get; private set; }

  // a list of all IPlaceable that have their origin in this tile
  public List<IPlaceable> Contains { get; private set; }

  // a list of all IPlaceables that have their origin in *other* GridTiles that intersect/overlap with this GridTile
  public List<IPlaceable> Intersects { get; private set; }

  public void AddPlaceable(IPlaceable placeable)
  {
    Contains.Add(placeable);
  }

  public void RemovePlaceable(IPlaceable placeable)
  {
    Contains.Remove(placeable);
  }

  public void AddIntersect(IPlaceable placeable)
  {
    Intersects.Add(placeable);
  }
  
  public void RemoveIntersect(IPlaceable placeable)
  {
    Intersects.Remove(placeable);
  }

  public void SetScale(Vector2 value)
  {
    transform.localScale = new Vector3(value.x, 1, value.y);
  }
  
  public void SetScenePosition(Vector2 value)
  {
    transform.position = new Vector3(value.x, 0, value.y);

    Boundary = new Rectangle(
      (int)transform.position.x,
      (int)transform.position.y,
      (int)transform.localScale.x,
      (int)transform.localScale.y
    );
  }

  public void SetPosition(Vector2 value)
  {
    Position = value;
  }

  private void Awake()
  {
    Contains = new();
    Intersects = new();
  }
}