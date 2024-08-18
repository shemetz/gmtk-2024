using System;
using System.Collections;
using System.Collections.Generic;
using GMTK.PlatformerToolkit;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class GeckoController : AnimalController
{

    [SerializeField] private float forwardSpeed = 5f;
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private Collider2D normalCollider, jumpCollider;
    [SerializeField] private MMF_Player JumpAnimation;
    private Rigidbody2D rb2d;
    private Animator myAnimator;

    private float jumpTimer;
    private bool isJumping = false;
    private float originalSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        myAnimator = GetComponentInChildren<Animator>();
        originalSpeed = forwardSpeed;
        normalCollider.enabled = true;
        jumpCollider.enabled = false;
    }

    protected override void Update()
    {
        base.Update();
        if (jumpTimer > 0)
        {
            jumpTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        rb2d.velocity = (transform.up * (forwardSpeed * Time.fixedDeltaTime));
    }

    protected override void NoInput()
    {
        /*if (transform.localRotation.eulerAngles != Vector3.zero)
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.identity, 5 * Time.deltaTime);
        }*/
    }

    protected override void LeftInput()
    {
        if (!isJumping)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }

    protected override void RightInput()
    {
        if (!isJumping)
        {
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }
    }   

    protected override void Jump()
    {
        if (jumpTimer <= 0)
        {
            JumpTask();
            jumpTimer = 10f;
        }
    }

    private async void JumpTask()
    {
        isJumping = true;
        normalCollider.enabled = false;
        jumpCollider.enabled = true;
        forwardSpeed *= 1.6f;
        myAnimator.speed = 0;
        await JumpAnimation.PlayFeedbacksTask();
        normalCollider.enabled = true;
        jumpCollider.enabled = false;
        forwardSpeed = originalSpeed;
        myAnimator.speed = 1;
        isJumping = false;
        jumpTimer = 1f;
    }
}