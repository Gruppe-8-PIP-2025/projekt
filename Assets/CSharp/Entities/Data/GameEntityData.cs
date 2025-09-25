using UnityEngine;

/// <author>
/// Can Özbal (canoezbal@gmail1.com)
/// </author>
/// <summary>
/// Stores data for different game entities. 
/// Only public so Unity can create ScriptableObjects.
/// </summary>
[CreateAssetMenu(fileName = "NewGameEntityData", menuName = "Game/EntityData")]
public class GameEntityData : ScriptableObject
{
    [Header("Shared Properties")]

    /// <summary>
    /// The GameObject representing the entity.
    /// </summary>
    public GameObject asset;

    /// <summary>
    /// Offset applied to the entity when instantiated.
    /// </summary>
    public Vector3 assetOffset = Vector3.zero;

    /// <summary>
    /// Rotation applied to the entity when instantiated.
    /// </summary>
    public Quaternion rotation = Quaternion.identity;

    /// <summary>
    /// Scale applied to the entity when instantiated.
    /// </summary>
    public Vector3 scale = Vector3.one;
}
