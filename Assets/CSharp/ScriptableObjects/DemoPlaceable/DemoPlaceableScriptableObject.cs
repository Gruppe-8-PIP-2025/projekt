using UnityEngine;

[CreateAssetMenu(fileName = "DemoPlaceable", menuName = "ScriptableObjects/DemoPlaceable", order = 1)]
public class DemoPlaceableScriptableObject : ScriptableObject
{
  [SerializeField] private string localizedName;
  [SerializeField] private GameObject asset;
  [SerializeField] private Vector3 positionAdjustment;
  [SerializeField] private Quaternion rotationAdjustment;
  [SerializeField] private Vector2 gridTile;

  public string LocalizedName => localizedName;
  public GameObject Asset => asset;
  public Vector2 GridTile => gridTile;

  public GameObject GameObject { get; set; }

  public void ApplyPosition()
  {
    GameObject.transform.position += positionAdjustment;
  }

  public void ApplyRotation()
  {
    GameObject.transform.rotation *= rotationAdjustment;
  }

  public void SetGridTile(GridTile position)
  {
    gridTile = position.Position;
  }

  public void InstantiateTo(GridTile position)
  {
    SetGridTile(position);
    GameObject.transform.SetParent(position.gameObject.transform);
    ApplyPosition();
    ApplyRotation();
  }
}
