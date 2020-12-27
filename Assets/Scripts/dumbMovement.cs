using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Connection;

public class dumbMovement : NetworkedBehaviour
{
    private void Start()
    {        
        if (NetworkedObject.IsOwner)
        {    
            DestroyImmediate(Camera.main.gameObject);
            GameObject cam = new GameObject();
            cam.name = "PlayerCamera";
            cam.transform.parent = this.transform;
            cam.transform.localPosition = new Vector3(0, 0, -10);
            cam.AddComponent<Camera>().orthographic = true;
        }
    }
    void Update()
    {
        if (NetworkedObject.IsLocalPlayer)
        {
            this.transform.position += new Vector3(0, Input.GetAxis("Vertical")) * 0.05f;
            this.transform.position += new Vector3(Input.GetAxis("Horizontal"), 0) * 0.05f;
            //this.GetComponent<Trac>
        }
    }
}
