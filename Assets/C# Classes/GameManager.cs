using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
    //Gamemanager to be used for server side checking
    public List<PlayerController> mPLayerList;
    // Use this for initialization
    void Start()
    {
        this.enabled = isServer;
        if (isServer)
        {
            mPLayerList = new List<PlayerController>(FindObjectsOfType<PlayerController>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < mPLayerList.Count; i++)
        {
            if (mPLayerList[i].Health < 0)
            {
                Debug.Log("Player dead");
            }
        }
    }

}
