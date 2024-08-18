using System;
using System.Collections;
using System.Collections.Generic;
using GMTK.PlatformerToolkit;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class EagleController : AnimalController
{
    public AudioClip screechSound;
    
    [SerializeField] private float forwardSpeed = 20f;
    [SerializeField] private float verticalSpeed = 10f;
    [SerializeField] private MMF_Player SquashStretch;
    private AudioSource _audio;
    private Rigidbody2D _rb2d;
    private Animator _anim;
    
    int attack = Animator.StringToHash("attack");
    
    // Start is called before the first frame update
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _audio = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();

        _rb2d.velocity = transform.right * forwardSpeed;
    }

    protected override void Update()
    {
        base.Update();
        
    }

    private void FixedUpdate()
    {
    }

    protected override void NoInput()
    {
        /*if (transform.localRotation.eulerAngles != Vector3.zero)
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.identity, 5 * Time.deltaTime);
        }*/
    }

    protected override void UpInput()
    {
        transform.Translate(Vector2.up * verticalSpeed * Time.deltaTime);
    }

    protected override void DownInput()
    {
        transform.Translate(Vector2.down * verticalSpeed * Time.deltaTime);
    }   

    protected override void Jump()
    {
        //do dash
        if (SquashStretch)
        {
            SquashStretch.PlayFeedbacks();
        }
    }

    protected override void Interact()
    {
        // claws forward
        _anim.SetTrigger(attack);
        //do screech
        _audio.PlayOneShot(screechSound);
    }
}