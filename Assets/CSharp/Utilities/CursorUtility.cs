using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

/// <author>
/// Can Özbal (canoezbal@gmail.com)
/// </author>
/// <summary>
/// Provides utility methods for handling the cursor in Unity, such as:
/// - Converting mouse position to world-space X/Z coordinates
/// - Setting and resetting cursor textures
/// - Storing a default cursor
/// </summary>
/// <remarks>
/// Attach this script to a GameObject in your Unity scene and assign
/// a Camera reference in the Inspector. Useful for RTS-style controls
/// or any system where the cursor position in world space is needed.
/// <br/><br/>!NOTE Removed static modifier from fields and methods.<br/>-Maria
/// </remarks>
public class CursorUtility : MonoBehaviour
{
    [Header("References")]
    /// <summary>
    /// The camera used to convert screen-space mouse position into a world-space ray.
    /// </summary>
    [SerializeField] private Camera cam;

    /// <summary>
    /// Stores the default cursor texture, if one is set.
    /// </summary>
    private Texture defaultCursorTexture = null;

    /// <author>
    /// Can Özbal (canoezbal@gmail.com)
    /// </author>
    /// <summary>
    /// Logs the current cursor world position (X and Z coordinates) to the Unity console.
    /// </summary>
    /// <param name="ctx">The input callback context, typically provided by Unity's Input System.</param>
    public void PrintPosition2D(CallbackContext ctx)
    {
        Debug.Log($"Cursor is located at X:{Position2D.x} Z:{Position2D.y}");
    }

    /// <summary>
    /// Gets the current cursor position projected onto the XZ plane (world space).
    /// </summary>
    /// <remarks>
    /// Requires the <see cref="cam"/> field to be assigned. If no intersection
    /// with the ground plane is found, returns <see cref="Vector2.zero"/>.
    /// </remarks>
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

    /// <summary>
    /// Sets the cursor texture to the specified <see cref="Texture2D"/>.
    /// </summary>
    /// <param name="texture">The texture to use for the cursor. Must be a <see cref="Texture2D"/>.</param>
    /// <remarks>
    /// If a <see cref="Sprite"/> is used, convert it to a <see cref="Texture2D"/> first.
    /// </remarks>
    public void SetCursorTexture(Texture texture)
    {
        if (texture is Texture2D tex2D)
        {
            Cursor.SetCursor(tex2D, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Debug.LogWarning("CursorUtility: Unity only supports Texture2D for Cursor.SetCursor(). If you have a Sprite, use its Texture2D.");
        }
    }

    /// <summary>
    /// Resets the cursor to the default texture if set, otherwise resets to system default.
    /// </summary>
    public void ResetCursorTexture()
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

    /// <summary>
    /// Defines a default cursor texture to be used by <see cref="ResetCursorTexture"/>.
    /// </summary>
    /// <param name="texture">The texture to store as the default cursor.</param>
    public void SetDefaultCursor(Texture texture)
    {
        defaultCursorTexture = texture;
    }
}


