using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private float Size;
    public bool selected;
    public Vector2 size { get; }

    public void setSelected(bool sel)
    {
        selected = sel;
        transform.GetChild(0).gameObject.SetActive(sel);
    }

    public float getSize()
    {
        return Size;
    }
}
