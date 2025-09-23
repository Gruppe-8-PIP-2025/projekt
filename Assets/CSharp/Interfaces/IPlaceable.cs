using UnityEngine;

/// <author>Maria Wickes (maria.lindling@protonmail.com)</author>
/// <summary>
/// The interface implemented by placeable objects that the GridManager uses to
/// organize them.
/// </summary>
public interface IPlaceable
{
  /// <summary>
  /// Describes the type of IPlaceable that implements this interface.
  /// </summary>
  public Placeables Category { get; }

  /// <summary>
  /// Describes the size and position of the IPlaceable in unity's scene coordinates.
  /// </summary>
  public Transform Transform { get; }

  /// <summary>
  /// Describes the size of the IPlaceable in grid spaces.
  /// </summary>
  public Vector2 Dimensions { get; }
}