using UnityEngine;

[System.Serializable]
public class MenuScreen
{
    [SerializeField] private string name;
    [SerializeField] private GameObject screenObject;

    public string Name => name;
    public GameObject ScreenObject => screenObject;

    public void Toggle()
    {
        if (screenObject != null)
            screenObject.SetActive(!screenObject.activeSelf);
    }

    public void Show()
    {
        if (screenObject != null) screenObject.SetActive(true);
    }

    public void Hide()
    {
        if (screenObject != null) screenObject.SetActive(false);
    }
}
