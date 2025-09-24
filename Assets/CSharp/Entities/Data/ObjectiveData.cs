using UnityEngine;

[CreateAssetMenu(fileName = "NewObjective", menuName = "Game Entities/Objective")]
public class ObjectiveData : GameEntityData
{
    [Header("Objective Properties")]
    public string description;
    public bool isPrimaryObjective;
    public int requiredAmount;
}
