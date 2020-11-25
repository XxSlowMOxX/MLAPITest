using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class NetworkingUI : MonoBehaviour
{
    public string adress = "127.0.0.1";
    public int port = 7777;
    void OnGUI()
    {        
        if (GUI.Button(new Rect(10, 10, 150, 25), "Host Server"))
        {
            NetworkingManager.Singleton.StartHost();
        }
        if (GUI.Button(new Rect(10, 40, 150, 25), "Join Server"))
        {
            NetworkingManager.Singleton.StartClient();
        }

    }
}
