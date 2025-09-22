using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

[System.Serializable]
public class GridDimensions
{
  // X positive is right
  // Z positive is up

  [SerializeField] private int tileSizeX;
  [SerializeField] private int tileSizeZ;

  [SerializeField] private int sizeX;
  [SerializeField] private int sizeZ;

  private int OriginX => -1 * (tileSizeX * sizeX / 2);
  private int OriginZ => tileSizeZ * sizeZ / 2;
  private int SizeX => tileSizeX * sizeX;
  private int SizeZ => tileSizeZ * sizeZ;

  public Rectangle Value => new(OriginX, OriginZ, SizeX, SizeZ);
  public Vector2 GridSize => new(sizeX, sizeZ);
  public Vector2 TileDimensions => new(tileSizeX, tileSizeZ);
}