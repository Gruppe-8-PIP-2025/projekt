using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;

    public GridLayout gridLayout;
    private Grid grid;
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private TileBase whiteTile;

    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;

    private PlaceObject objectToPlace;



    #region Unity Methods

    private void Awake()
    {
        // Singleton pattern
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }

    private void Update()
    {
        // Handle input for placing different prefabs
        if (Input.GetKeyDown(KeyCode.A)) 
        {
            InitializeWithObject(prefab1);
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            InitializeWithObject(prefab2);
        }
        else if (Input.GetKeyDown(KeyCode.C)) 
        {
            InitializeWithObject(prefab3);
        }
    }

    #endregion


    #region Utils

    public static Vector3 GetMouseWorldPosition()
    {
        // Create a ray from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        // Convert the world position to a cell position and then back to the center of that cell
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }

    #endregion


    #region Building Placement

    public void InitializeWithObject(GameObject prefab)
    {
       
        Vector3 position = SnapCoordinateToGrid(Vector3.zero);

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace = obj.GetComponent<PlaceObject>();
        obj.AddComponent<DragObject>();
    }
    
    #endregion

}
