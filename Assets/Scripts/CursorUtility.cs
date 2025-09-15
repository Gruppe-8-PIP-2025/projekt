using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CursorUtility : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;   

    private static Texture defaultCursorTexture = null;

    public void PrintPosition2D(CallbackContext ctx)
    {
        Debug.Log($"Cursor is located at X:{Position2D.x} Z:{Position2D.y}");
    }

    public Vector2 Position2D   
    {
        get
        {
            if (cam == null)
                throw new NullReferenceException("You must assign a Camera to CursorUtility!");

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            if (groundPlane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                return new Vector2(hitPoint.x, hitPoint.z);
            }

            return Vector2.zero;
        }
    }

    public static void SetCursorTexture(Texture texture)
    {
        if (texture is Texture2D tex2D)
        {
            Cursor.SetCursor(tex2D, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Debug.LogWarning("CursorUtility: Unity unterstützt nur Texture2D für Cursor.SetCursor(). Falls du ein Sprite hast, musst du dessen Texture2D verwenden.");
        }
    }

    public static void ResetCursorTexture()
    {
        if (defaultCursorTexture != null && defaultCursorTexture is Texture2D tex2D)
        {
            Cursor.SetCursor(tex2D, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    public static void SetDefaultCursor(Texture texture)
    {
        defaultCursorTexture = texture;
    }

    //private void Update()
    //{
    //    // Wenn linke Maustaste gedrückt wird
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        RaycastHit raycastHit;

    //        // Ray von diesem Objekt nach vorne
    //        if (Physics.Raycast(transform.position, transform.forward, out raycastHit))
    //        {
    //            if (raycastHit.collider.CompareTag("Respawn"))
    //            {
    //                Debug.DrawRay(transform.position, transform.forward * raycastHit.distance, Color.green, 10f);
    //                Debug.Log("raycastHit: " + raycastHit.collider.name);
    //            }
    //        }
    //    }
    //}
}
