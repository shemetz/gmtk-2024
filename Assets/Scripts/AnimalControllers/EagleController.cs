using System;
using System.Collections;
using System.Collections.Generic;
using GMTK.PlatformerToolkit;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class EagleController : AnimalController
{

    [SerializeField] private float forwardSpeed = 20f;
    [SerializeField] private float verticalSpeed = 10f;
    [SerializeField] private MMF_Player SquashStretch;
    [SerializeField] private MMF_Player AudioPlayer;
    private Rigidbody2D rb2d;
    private Animator myAnimator;

    private float originalSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        myAnimator = GetComponentInChildren<Animator>();
        originalSpeed = forwardSpeed;
    }

    protected override void Update()
    {
        base.Update();
        
    }

    private void FixedUpdate()
    {
        rb2d.velocity = (transform.right * (forwardSpeed * Time.fixedDeltaTime));
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
        //do screech

        if (AudioPlayer)
        {
            AudioPlayer.PlayFeedbacks();
        }
    }
}