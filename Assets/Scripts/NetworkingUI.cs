using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Transports.UNET;
using MLAPI.SceneManagement;
using MLAPI.Connection;
using MLAPI.NetworkedVar;

public class NetworkingUI : NetworkedBehaviour
{
    public string adress = "127.0.0.1";
    public string port = "7777";
    private NetworkManager nm;
    void Start()
    {
        nm = this.GetComponent<NetworkManager>();
    }
    void OnGUI()
    {
        if(NetworkingManager.Singleton.IsServer|| NetworkingManager.Singleton.IsConnectedClient|| NetworkingManager.Singleton.IsHost)
        {
            if (NetworkingManager.Singleton.IsHost)
            {
                if(GUI.Button(new Rect(10, 10, 200, 25),"Next Level")) {
                    NetworkSceneManager.SwitchScene("testScene");
                }
                int i = 0;
                foreach (KeyValuePair<ulong,NetworkedClient> entry in NetworkingManager.Singleton.ConnectedClients) {
                    GUI.Label(new Rect(10, 40 + (i * 30), 200, 25), entry.Value.ClientId.ToString()); i++;
                }
            }
            return;
        }
        else //If neither Server Client nor Host e.g. not Connected
        {
            adress = GUI.TextField(new Rect(10, 70, 125, 25), adress);
            port = GUI.TextField(new Rect(135, 70, 75, 25), port);
            if (GUI.Button(new Rect(10, 10, 200, 25), "Host Server"))
            {
                nm.HostServer();               
            }
            if (GUI.Button(new Rect(10, 40, 200, 25), "Join Server"))
            {
                nm.StartClient(adress, port);
            }
        } 
    }     
}