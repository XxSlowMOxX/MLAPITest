using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Transports.UNET;
using MLAPI.SceneManagement;

public class NetworkingUI : MonoBehaviour
{
    public string adress = "127.0.0.1";
    public string port = "7777";
    void OnGUI()
    {
        if(NetworkingManager.Singleton.IsServer|| NetworkingManager.Singleton.IsConnectedClient|| NetworkingManager.Singleton.IsHost)
        {
            if (NetworkingManager.Singleton.IsHost)
            {
                if(GUI.Button(new Rect(10, 10, 200, 25),"Next Level")) {
                    NetworkSceneManager.SwitchScene("testScene");
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
                NetworkingManager.Singleton.StartHost();
            }
            if (GUI.Button(new Rect(10, 40, 200, 25), "Join Server"))
            {
                NetworkingManager.Singleton.GetComponent<UnetTransport>().ConnectAddress = adress;
                NetworkingManager.Singleton.GetComponent<UnetTransport>().ConnectPort = int.Parse(port);
                NetworkingManager.Singleton.StartClient(); //This Comment is to test the new .Gitignore
            }
        } 
    }
    
}
