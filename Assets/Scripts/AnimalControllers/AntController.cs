using System.Collections;
using System.Collections.Generic;
using GMTK.PlatformerToolkit;
using UnityEngine;
using UnityEngine.InputSystem;

public class AntController : AnimalController
{
    public AudioClip movementSound;
    
    [SerializeField] private characterMovement _characterMovement;
    private Rigidbody2D rb2d;
    private AudioSource _audio;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        _audio = GetComponent<AudioSource>();
    }

    /*protected override void Update()
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
    }*/

    protected override void NoInput()
    {
        _characterMovement.OnMovement(0);
        
        if (_audio.isPlaying)
        {
            _audio.Stop();
        }
    }

    protected override void LeftInput()
    {
        _characterMovement.OnMovement(-1);
        playMovementSound();
    }

    protected override void RightInput()
    {
        _characterMovement.OnMovement(1);
        playMovementSound();
    }

    private void playMovementSound()
    {
        if (!_audio.isPlaying)
        {
            _audio.loop = true;
            _audio.clip = movementSound;
            _audio.Play();
        }
    }
}
