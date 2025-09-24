using UnityEngine;

public  class GameEntityData : ScriptableObject
{
    [Header("Shared Properties")]
    public GameObject asset;
    public Vector3 assetOffset = Vector3.zero;
    public Quaternion rotation = Quaternion.identity;
    public Vector3 scale = Vector3.one;
}
