using UnityEngine;

/// <author>
/// Can Özbal (canoezbal@gmail1.com)
/// </author>
/// <summary>
/// Stores data specific to objectives in the game.
/// Inherits from GameEntityData.
/// </summary>
[CreateAssetMenu(fileName = "NewObjective", menuName = "Game Entities/Objective")]
public class ObjectiveData : GameEntityData
{
    [Header("Objective Properties")]

    /// <summary>
    /// Description of the objective.
    /// </summary>
    public string description;

    /// <summary>
    /// Whether this objective is a primary objective.
    /// </summary>
    public bool isPrimaryObjective;

    /// <summary>
    /// Number of items or actions required to complete the objective.
    /// </summary>
    public int requiredAmount;
}
