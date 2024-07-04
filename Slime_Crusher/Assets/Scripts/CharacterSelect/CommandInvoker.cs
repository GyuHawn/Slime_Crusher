using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker : MonoBehaviour
{
    private Queue<Command> commandQueue = new Queue<Command>();

    public void AddCommand(Command command)
    {
        commandQueue.Enqueue(command);
    }
    
    public void ExecuteCommands()
    {
        while (commandQueue.Count > 0)
        {
            Command command = commandQueue.Dequeue();
            command.Execute();
        }
    }
}
