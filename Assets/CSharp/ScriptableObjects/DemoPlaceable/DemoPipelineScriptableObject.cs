using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DemoPipeline", menuName = "ScriptableObjects/DemoPipeline", order = 1)]
public class DemoPipelineScriptableObject : ScriptableObject
{
  private enum Orientation
  {
    Horizontal,
    Vertical
  }
  private readonly Vector3 _centerOffset = new(-5.0f, -0.0f, -5.0f); 
  [SerializeField] private string localizedName;
  [SerializeField] private GameObject asset;
  [SerializeField] private Vector3 positionAdjustment;
  [SerializeField] private Quaternion rotationAdjustment;
  [SerializeField] private Vector2 gridTile;
  [SerializeField] private Orientation orientation;
  [SerializeField] private int unitLength;

  public string LocalizedName => localizedName;
  public GameObject Asset => asset;
  public Vector2 GridTile => gridTile;

  public List<GameObject> GameObjects { get; }

  public void ApplyPosition()
  {
    GameObjects.ForEach(go=>go.transform.position += positionAdjustment + _centerOffset);

    for (int i = 0; i < GameObjects.Count; i++)
    {
      GameObjects[i].transform.position += (
        orientation == Orientation.Horizontal ?
            new Vector3(1.0f, 0.0f, 0.0f)
          : new Vector3(0.0f, 0.0f, 1.0f)
      );
    }
  }

  public void ApplyRotation()
  {
    GameObjects.ForEach(go=>go.transform.rotation *= rotationAdjustment);
  }

  public void SetGridTile(GridTile position)
  {
    gridTile = position.Position;
  }
}
