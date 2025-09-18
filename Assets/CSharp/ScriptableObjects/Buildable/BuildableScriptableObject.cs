using UnityEngine;

[CreateAssetMenu(fileName = "Buildable", menuName = "ScriptableObjects/BuildableScriptableObject", order = 1)]
public class BuildableScriptableObject : ScriptableObject
{
  [SerializeField] private string localizedName;
  [SerializeField] private Texture menuIcon;

  public string LocalizedName => localizedName;
  public Texture MenuIcon => menuIcon;
}