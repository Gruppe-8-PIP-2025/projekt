using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
class ButtonConfig
{
  [SerializeField] private GameObject buttonImageComponent;
  [SerializeField] private Texture2D texture;

  public void Apply()
  {
    buttonImageComponent.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.0f, 0.0f));
  }
}