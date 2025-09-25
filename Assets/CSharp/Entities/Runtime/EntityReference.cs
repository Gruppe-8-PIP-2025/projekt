using UnityEngine;

/// <author>
/// Can Özbal (canoezbal@gmail1.com)
/// </author>
/// <summary>
/// Holds a reference to a GameEntityData for a specific GameObject in the scene.
/// </summary>
public class EntityReference : MonoBehaviour
{
    /// <summary>
    /// ScriptableObject containing data for this entity.
    /// </summary>
    public GameEntityData data;
}
