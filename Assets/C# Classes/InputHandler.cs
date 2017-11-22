using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
namespace CommandPatterns
{
    public class NetworkHandler : NetworkBehaviour
    {

    }

    public class InputHandler : MonoBehaviour
    {

        public Transform _transform;

        //WASD for movement, Z = undo, R = redo
        private Command buttonW, buttonS, buttonA, buttonD, buttonB, buttonZ, buttonR;

        //store the old commands so I can undo/redo
        public static List<Command> oldCommands = new List<Command>();

        private Vector3 boxStartPosition;

        //To reset the coroutine
        private Coroutine replayCoroutine;

        public static bool shouldStartReplay;

        private bool isReplaying;


        // Use this for initialization
        void Start()
        {
            buttonB = new DoNothing();
            buttonW = new MoveForward();
            buttonS = new MoveBack();
            buttonA = new MoveLeft();
            buttonD = new MoveRight();
            buttonZ = new Undo();
            buttonR = new ReplayCommand();
            
            boxStartPosition = _transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (!isReplaying)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    buttonA.Execute(_transform, buttonA);
                }
                else if (Input.GetKeyDown(KeyCode.B))
                {
                    buttonB.Execute(_transform, buttonB);
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    buttonD.Execute(_transform, buttonD);
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    buttonS.Execute(_transform, buttonS);
                }
                else if (Input.GetKeyDown(KeyCode.W))
                {
                    buttonW.Execute(_transform, buttonW);
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    buttonZ.Execute(_transform, buttonZ);
                }
                else if (Input.GetKeyDown(KeyCode.R))
                {
                    buttonR.Execute(_transform, buttonZ);
                }
            }
            StartReplay();
        }

        void StartReplay()
        {
            if (shouldStartReplay && oldCommands.Count > 0)
            {
                shouldStartReplay = false;

                if (replayCoroutine != null)
                {
                    StopCoroutine(replayCoroutine);
                }

                replayCoroutine = StartCoroutine(ReplayCommands(_transform));
            }
        }

        IEnumerator ReplayCommands(Transform boxTrans)
        {
            isReplaying = true;
            boxTrans.position = boxStartPosition;

            for (int i =0; i < oldCommands.Count; i++)
            {
                oldCommands[i].Move(boxTrans);

                yield return new WaitForSeconds(0.3f);
            }

            isReplaying = false;
        }
    }

    public abstract class Command
    {
        protected float moveDistance = 1f;

        public abstract void Execute(Transform boxTrans, Command command);
        public virtual void Move(Transform boxTrans) { }
        public virtual void Undo(Transform boxTrans) { }
    }


    public class DoNothing : Command
    {
        public override void Execute(Transform boxTrans, Command command)
        {
            //nothing
        }
    }

    public class MoveForward : Command
    {
        public override void Execute(Transform boxTrans, Command command)
        {
            Move(boxTrans);
            InputHandler.oldCommands.Add(command);
        }

        public override void Move(Transform boxTrans)
        {
            boxTrans.Translate(boxTrans.forward * moveDistance);
        }

        public override void Undo(Transform boxTrans)
        {
            boxTrans.Translate(-boxTrans.forward * moveDistance);
        }
    }

    public class MoveLeft : Command
    {
        public override void Execute(Transform boxTrans, Command command)
        {
            Move(boxTrans);
            InputHandler.oldCommands.Add(command);
        }

        public override void Move(Transform boxTrans)
        {
            boxTrans.Translate(boxTrans.right * moveDistance);
        }

        public override void Undo(Transform boxTrans)
        {
            boxTrans.Translate(-boxTrans.right * moveDistance);
        }
    }

    public class MoveRight : Command
    {
        public override void Execute(Transform boxTrans, Command command)
        {
            Move(boxTrans);
            InputHandler.oldCommands.Add(command);
        }

        public override void Move(Transform boxTrans)
        {
            boxTrans.Translate(-boxTrans.right * moveDistance);
        }

        public override void Undo(Transform boxTrans)
        {
            boxTrans.Translate(boxTrans.right * moveDistance);
        }
    }

    public class MoveBack : Command
    {
        public override void Execute(Transform boxTrans, Command command)
        {
            Move(boxTrans);
            InputHandler.oldCommands.Add(command);
        }

        public override void Move(Transform boxTrans)
        {
            boxTrans.Translate(-boxTrans.forward * moveDistance);
        }

        public override void Undo(Transform boxTrans)
        {
            boxTrans.Translate(boxTrans.forward * moveDistance);
        }
    }

    public class Undo : Command
    {
        public override void Execute(Transform boxTrans, Command command)
        {
            List<Command> oldCommands = InputHandler.oldCommands;
            if (oldCommands.Count > 0)
            {
                Command latestCommand = oldCommands[oldCommands.Count - 1];
                latestCommand.Undo(boxTrans);
                oldCommands.RemoveAt(oldCommands.Count - 1);
            }
        }

    }

    public class ReplayCommand : Command
    {
        public override void Execute(Transform boxTrans, Command command)
        {
            InputHandler.shouldStartReplay = true;
        }

    }
}
