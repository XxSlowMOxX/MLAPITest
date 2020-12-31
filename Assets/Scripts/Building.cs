using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private float Size;
    public bool selected;

    public void setSelected(bool sel)
    {
        selected = sel;
        GetComponentInChildren<SpriteRenderer>().gameObject.SetActive(sel);
    }

    public float getSize()
    {
        return Size;
    }
}
