using UnityEngine;

public class TestPlaceable : MonoBehaviour, IPlaceable
{
  [SerializeField] private Placeables category;
  [SerializeField] private Vector2 dimensions;

  public Placeables Category => category;
  public Transform Transform => gameObject.transform;
  public Vector2 Dimensions => dimensions != default ? dimensions : new Vector2(1.0f, 1.0f);

  public GameObject ComponentOf => gameObject;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
      
  }

  // Update is called once per frame
  void Update()
  {
      
  }
}
