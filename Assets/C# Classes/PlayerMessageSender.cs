using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
public class PlayerMessageSender : NetworkBehaviour
{
    NetworkClient mClient;

    void Start()
    {
        SetupClient();
    }

    public class MyMsgType
    {
        public static short Score = MsgType.Highest + 1; //the message ID
    }

    public class ScoreMessage : MessageBase
    {
        public int score;
        public Vector3 scorePos;
        public int lives;
    }

    public void SendScore(int score, Vector3 scorePos, int lives)
    {
        ScoreMessage msg = new ScoreMessage();
        msg.score = score;
        msg.scorePos = scorePos;
        msg.lives = lives;
        NetworkServer.SendToAll(MyMsgType.Score, msg);
    }

    public void SetupClient()
    {
        mClient = new NetworkClient();
        mClient.RegisterHandler(MsgType.Connect, OnConnected);
        mClient.RegisterHandler(MyMsgType.Score, OnScore);
        //mClient.Connect("127.0.0.1", 7777);
    }

    public void OnScore(NetworkMessage netMsg)
    {
        ScoreMessage msg = netMsg.ReadMessage<ScoreMessage>(); //deserialize
        Debug.Log("OnScoreMessage " + msg.score);
    }

    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to server");
    }
}
