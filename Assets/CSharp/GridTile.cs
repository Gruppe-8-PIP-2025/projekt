using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

/// <author>Maria Wickes (maria.lindling@protonmail.com)</author>
/// <summary>
/// Class of which each instance represents a specific tile within a grid. 
/// </summary>
public class GridTile : MonoBehaviour
{
  #region Unity Editor Fields
  /// <summary>
  /// The in-scene GameObject (plane) that represents the GridTile itself.
  /// </summary>
  [SerializeField] private GameObject plane;
  #endregion


  #region Properties
  /// <summary>
  /// Describes the tile's position in the grid.
  /// </summary>
  /// <remarks><b>WARNING:</b> <i>Y here is Z in Unity!</i></remarks>
  public Vector2 Position { get; private set; }

  /// <summary>
  /// A tuple containing the coordinates of each grid-coordinate that is
  /// adjacent to this GridTile.
  /// </summary>
  /// <remarks><b>WARNING:</b> <i>Y here is Z in Unity!</i></remarks>
  public (
    Vector2 TopLeft, Vector2 Top, Vector2 TopRight,
    Vector2 Left, Vector2 Right,
    Vector2 BottomLeft, Vector2 Bottom, Vector2 BottomRight
    ) Adjacencies =>
    (
      Position + new Vector2(-1, 1),
      Position + new Vector2(0, 1),
      Position + new Vector2(1, 1),
      Position + new Vector2(-1, 0),
      Position + new Vector2(1, 0),
      Position + new Vector2(-1, -1),
      Position + new Vector2(0, -1),
      Position + new Vector2(1, -1)
    );

  /// <summary>
  /// A tuple containing the coordinates of each grid-coordinate that is
  /// adjacent to this GridTile.
  /// </summary>
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

  /// <summary>
  /// Describes the tile's size and position in unity's scene coordinates.
  /// </summary>
  public RectangleF Boundary { get; private set; }

  /// <summary>
  /// A list of all IPlaceable that have their origin in this tile.
  /// </summary>
  public List<IPlaceable> Contains { get; private set; }

  /// <summary>
  /// A list of all IPlaceables that have their origin in *other* GridTiles
  /// that intersect/overlap with this GridTile.
  /// </summary>
  public List<IPlaceable> Intersects { get; private set; }
  #endregion


  #region Add/Remove Placeables 
  /// <summary>
  /// Adds an IPlaceable to this GridTile.
  /// </summary>
  /// <param name="placeable">The IPlaceable to be added to this GridTile.</param>
  public void AddPlaceable(IPlaceable placeable)
  {
    Contains.Add(placeable);
  }

  /// <summary>
  /// Removes an IPlaceable from this GridTile.
  /// </summary>
  /// <param name="placeable">
  /// The IPlaceable to be removed from to this GridTile.
  /// </param>
  public void RemovePlaceable(IPlaceable placeable)
  {
    Contains.Remove(placeable);
  }

  /// <summary>
  /// Informs this GridTile, that the given IPlaceable intersects with it.
  /// </summary>
  /// <param name="placeable">The IPlaceable intersecting with this GridTile.</param>
  public void AddIntersect(IPlaceable placeable)
  {
    Intersects.Add(placeable);
  }

  /// <summary>
  /// Informs this GridTile, that the given IPlaceable no longer intersects with it.
  /// </summary>
  /// <param name="placeable">
  /// The IPlaceable no longer intersecting with this GridTile.
  /// </param>
  public void RemoveIntersect(IPlaceable placeable)
  {
    Intersects.Remove(placeable);
  }
  #endregion


  #region Geometry
  /// <summary>
  /// Sets the scale of the plane serving as the GridTile's in-scene representation.
  /// </summary>
  /// <param name="value">The new scale of the plane.</param>
  public void SetScale(float value)
  {
    plane.transform.localScale = new Vector3(value, 1.0f, value);
  }

  /// <summary>
  /// Sets the scale of the plane serving as the GridTile's in-scene representation.
  /// </summary>
  /// <param name="value">The new scale of the plane along its two axies.</param>
  /// <remarks><b>WARNING:</b> <i>Y here is Z in Unity!</i></remarks>
  public void SetScale(Vector2 value)
  {
    plane.transform.localScale = new Vector3(value.x, 1.0f, value.y);
  }

  /// <summary>
  /// Sets the position of this GridTile within the Scene.
  /// </summary>
  /// <param name="value">The new coordinates of the object along the two horizon axies.</param>
  /// <remarks><b>WARNING:</b> <i>Y here is Z in Unity!</i></remarks>
  public void SetScenePosition(Vector2 value)
  {
    transform.position = new Vector3(value.x, 0.0f, value.y);

    // TODO: check scale/coordinate relationship
    float scaleCoordRel = 10.0f;
    Boundary = new RectangleF(
      transform.position.x,
      transform.position.y,
      scaleCoordRel * transform.localScale.x,
      scaleCoordRel * transform.localScale.y
    );
    // --
  }

  /// <summary>
  /// Sets the GridTile's position within the grid itself.
  /// </summary>
  /// <param name="value">The new coordinates of the object along the two horizon axies.</param>
  /// <remarks><b>WARNING:</b> <i>Y here is Z in Unity!</i></remarks>
  public void SetPosition(Vector2 value)
  {
    Position = value;
  }
  #endregion


  #region MonoBehavior
  /// <summary>
  /// Awake is called once before the first execution of Update and Start after
  /// the MonoBehaviour is created. It will be called even if this GameObject is
  /// not active. 
  /// </summary>
  private void Awake()
  {
    Contains = new();
    Intersects = new();
  }
  #endregion
}