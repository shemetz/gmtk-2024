
using System;
using UnityEngine;
using UnityEngine.Events;

public class BrainButton : MonoBehaviour
{
    public BrainCommand brainCommand;
    
    public UnityAction<bool, BrainCommand> SendCommand;

    private void Start()
    {
        // EXAMPLES
        SendCommand?.Invoke(false, BrainCommand.AntRight);
        SendCommand?.Invoke(false, BrainCommand.AntLeft);
    }
}