using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public bool selected;
    private Vector2 size = Vector2.zero;

    public void setSelected(bool sel)
    {
        selected = sel;
        transform.GetChild(0).gameObject.SetActive(sel);
    }

    public Vector2 getSize()
    {
        if(size != Vector2.zero)
        {
            return size;
        }
        else //Calculates Size of Building
        { 
            return GetComponent<SpriteRenderer>().sprite.bounds.size;
        }
    }
}
