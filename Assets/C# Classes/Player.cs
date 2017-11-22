using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Receiver receiver;
    Command command;
    Invoker invoker;
    private void Start()
    {
        receiver = new Receiver();
        command = new MovementCommand(receiver);
        invoker = new Invoker();

        invoker.SetCommand(command);
        invoker.ExecuteCommand();

    }
}
