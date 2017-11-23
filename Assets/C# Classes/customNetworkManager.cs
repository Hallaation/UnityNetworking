using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//Network manager 
public class customNetworkManager : NetworkManager
{
    public List<PlayerController> test;
    void Awake()
    {
        test = new List<PlayerController>();
    }

    private void OnServerInitialized()
    {
        Debug.LogError("Server iniitalized");
    }


    void Update()
    {

    }

    //Called when a client connects to a server
    public override void OnClientConnect(NetworkConnection connection)
    {
       // Debug.LogError("Client connected " + connection.address);
    }
    //When a client connects to the server
    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        Debug.LogError("ID given to the client : " + conn.connectionId);

        //if (gm.isServer)
        //{
        //    Debug.LogError(conn.clientOwnedObjects);
        //}
    }
}
