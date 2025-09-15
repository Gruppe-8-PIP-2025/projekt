using UnityEngine;


public  class CursorUtility : MonoBehaviour
    
    {
    private static Texture defaultCursorTexture = null;

    
    public static Vector2 Position2D
    {
        get
        {
            Camera cam = Camera.main;
            if (cam == null) return Vector2.zero;

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


}
