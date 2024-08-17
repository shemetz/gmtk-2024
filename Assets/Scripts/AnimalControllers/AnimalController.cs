using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CommandType
{
    None,
    UpInput,
    DownInput,
    LeftInput,
    RightInput,
    Interact,
    Jump
}

public abstract class AnimalController : MonoBehaviour
{
    #region Singleton
    // create a private reference to T instance
    protected static AnimalController instance;

    public static AnimalController Instance
    {
        get
        {
            // if instance is null
            if (instance == null)
            {
                // find the generic instance
                instance = FindAnyObjectByType<AnimalController>();
            }
            return instance;
        }
    }

    public virtual void Awake()
    {
        // create the instance
        if (instance == null)
        {
            instance = this as AnimalController;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    

    #endregion

    protected CommandType currentCommand;
    
    // Start is called before the first frame update
    void Start()
    {
        currentCommand = CommandType.None;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        switch (currentCommand)
        {
            case CommandType.None:
                NoInput();
                break;
            case CommandType.UpInput:
                UpInput();
                break;
            case CommandType.DownInput:
                DownInput();
                break;
            case CommandType.LeftInput:
                LeftInput();
                break;
            case CommandType.RightInput:
                RightInput();
                break;
            case CommandType.Interact:
                Interact();
                break;
            case CommandType.Jump:
                Jump();
                break;
            default:
                Debug.LogWarning("Illegal command type: " + currentCommand);
                break;
        }
    }

    public virtual void SwitchCommand(bool toggleOn, CommandType commandType)
    {
        currentCommand = !toggleOn ? CommandType.None : commandType;
    }

    protected virtual void NoInput()
    {
        
    }

    protected virtual void UpInput()
    {
        
    }

    protected virtual void DownInput()
    {
        
    }

    protected virtual void LeftInput()
    {
        
    }

    protected virtual void RightInput()
    {
        
    }

    protected virtual void Interact()
    {
        
    }

    protected virtual void Jump()
    {
        
    }
}
