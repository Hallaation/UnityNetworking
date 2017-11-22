using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class NetworkUI : MonoBehaviour
{
    public NetworkManager manager;
    public string networkIP;
    public System.UInt16 networkPort;

    private void Awake()
    {
        manager = FindObjectOfType(typeof(NetworkManager)) as NetworkManager;
    }
    // Update is called once per frame

    private void Update()
    {
        if (NetworkServer.active && NetworkClient.active)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                manager.StopHost();
            }
        }
    }

    public void StartLocalServer()
    {
        manager.StartServer();
    }

    public void StartClient()
    {
        manager.StartClient();
    }

    public void StartHost()
    {
        manager.StartHost();
    }

    



}
