using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class dumbMovement : NetworkedBehaviour
{
    void Update()
    {
        if (NetworkedObject.IsLocalPlayer)
        {
            this.transform.position += new Vector3(0,Input.GetAxis("Vertical")) * 0.05f;
            this.transform.position += new Vector3(Input.GetAxis("Horizontal"), 0) * 0.05f;
        }
    }
}
