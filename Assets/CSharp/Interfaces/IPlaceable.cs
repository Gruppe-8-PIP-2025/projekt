using UnityEngine;

public interface IPlaceable
{
  // describes the type of IPlaceable that implements this interface
  public Placeables Category { get; }

  // describes the size and position of the IPlaceable in unity's scene coordinates
  public Transform Transform { get; }

  // describes the size of the IPlaceable in grid spaces
  public Vector2 Dimensions { get; }
}