
using UnityEngine;

[System.Serializable]
class BuildableEntry
{
  [SerializeField] private BuildableScriptableObject buildable;
  [SerializeField] private bool enabled;

  public BuildableScriptableObject Buildable => buildable;
  public bool Enabled => enabled;
}