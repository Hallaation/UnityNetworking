using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public abstract class Command
{
    protected Receiver receiver;

    public Command(Receiver receiver)
    {
        this.receiver = receiver;
    }
    
    public abstract void Execute();
    public abstract void UnExecute();
}

public class MovementCommand : Command
{
    public MovementCommand(Receiver receiver)
        : base(receiver) { }

    public override void Execute()
    {
        Debug.Log("MovementCommand.Execute() called");
    }

    public override void UnExecute()
    {
        Debug.Log("MovementCommand.UnExecute() called");
        GameObject.Find("Player").GetComponent<Rigidbody>().AddForce(Vector3.forward * 10, ForceMode.Acceleration);
    }
}


public class Receiver
{
    public void Action()
    {
        Debug.Log("Called receiver.Action()");
    }
}

public class Invoker
{
    private Command _command;

    public void SetCommand(Command command)
    {
        this._command = command;
    }

    public void ExecuteCommand()
    {
        _command.Execute();
    }
}




