using System.Drawing;
using UnityEngine;

public class gridboundyDUMMY : MonoBehaviour
{
    [SerializeField] private int topLeft_x;
    [SerializeField] private int topLeft_z;
    [SerializeField] private int bottomRight_x;
    [SerializeField] private int bottomRight_z;
    private Rectangle gridBounds;

    public bool IsPlayerInBounds()
    {
        return false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gridBounds = new Rectangle(topLeft_x, topLeft_z, bottomRight_x, bottomRight_z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
