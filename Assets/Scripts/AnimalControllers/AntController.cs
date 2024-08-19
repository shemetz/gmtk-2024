using System.Collections;
using System.Collections.Generic;
using GMTK.PlatformerToolkit;
using UnityEngine;
using UnityEngine.InputSystem;

public class AntController : AnimalController
{
    public AudioClip movementSound;
    
    [SerializeField] private characterMovement _characterMovement;
    [SerializeField] private GameObject _controlsUi;
    private AudioSource _audio;

    private bool _leftWasPressed;
    private bool _rightWasPressed;
    
    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    protected override void NoInput()
    {
        _characterMovement.OnMovement(0);
        
        if (_audio.isPlaying)
        {
            _audio.Stop();
        }

        if (_leftWasPressed && _rightWasPressed)
        {
            _controlsUi.SetActive(false);
        }
    }

    protected override void LeftInput()
    {
        _characterMovement.OnMovement(-1);
        playMovementSound();
        _leftWasPressed = true;
    }

    protected override void RightInput()
    {
        _characterMovement.OnMovement(1);
        playMovementSound();
        _rightWasPressed = true;
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
