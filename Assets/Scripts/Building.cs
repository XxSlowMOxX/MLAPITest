using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public bool selected;
    [SerializeField]
    private Vector2 size;

    public void setSelected(bool sel)
    {
        selected = sel;
        transform.GetChild(0).gameObject.SetActive(sel);
    }

    public Vector2 getSize()
    {
        return size;
    }
}
