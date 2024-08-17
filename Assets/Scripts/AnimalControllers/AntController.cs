using System.Collections;
using System.Collections.Generic;
using GMTK.PlatformerToolkit;
using UnityEngine;
using UnityEngine.InputSystem;

public class AntController : AnimalController
{
    [SerializeField] private characterMovement _characterMovement;
    private Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        base.Update();

        #region Debug
        if (!Input.anyKey)
        {
            currentCommand = CommandType.None;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            currentCommand = CommandType.UpInput;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            currentCommand = CommandType.DownInput;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            currentCommand = CommandType.RightInput;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            currentCommand = CommandType.LeftInput;
        }
        #endregion
    }

    protected override void NoInput()
    {
        _characterMovement.OnMovement(0);
    }

    protected override void LeftInput()
    {
        _characterMovement.OnMovement(-1);
    }

    protected override void RightInput()
    {
        _characterMovement.OnMovement(1);
    }
}
