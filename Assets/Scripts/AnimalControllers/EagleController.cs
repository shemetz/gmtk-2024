using System;
using System.Collections;
using System.Collections.Generic;
using GMTK.PlatformerToolkit;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class EagleController : AnimalController
{
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float rotationAngle;
    [SerializeField] private float minimumDashDuration;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float delayBetweenAttacks;
    [SerializeField] private MMF_Player SquashStretch;
    [SerializeField] private AudioClip[] screechSounds;
    [SerializeField] private AudioClip dashSound;
    
    private AudioSource _audio;
    private Animator _anim;
    private Rigidbody2D _rb;

    private Vector2 _moveInput;
    private float _timeSinceAttack = 999;
    private float _timeSinceDash = 999;
    private bool _dashInput = false;

    int attack = Animator.StringToHash("attack");

    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _anim = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // transform.Translate(forwardSpeed * Time.fixedDeltaTime * Vector2.right, Space.World);
        // transform.Translate(verticalSpeed * Time.fixedDeltaTime * _moveInput, Space.World);

        var horizontalSpeed = (_dashInput || _timeSinceDash < minimumDashDuration) ? dashSpeed : forwardSpeed;
        _rb.velocity = new Vector2(horizontalSpeed, verticalSpeed * _moveInput.y);
        
        // rotate a bit up or down or reset, depending on vertical move input
        // lerping slowly to avoid sudden changes
        float targetRotation = _moveInput.y * rotationAngle;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, targetRotation), Time.fixedDeltaTime * 20f);

        _timeSinceAttack += Time.fixedDeltaTime;
        _timeSinceDash += Time.fixedDeltaTime;
    }

    protected override void NoInput()
    {
        _moveInput = Vector2.zero;
        _dashInput = false;
    }

    protected override void UpInput()
    {
        _moveInput = Vector2.up;
    }

    protected override void DownInput()
    {
        _moveInput = Vector2.down;
    }

    protected override void Jump()
    {
        //do dash
        if (!_dashInput && _timeSinceDash > minimumDashDuration)
        {
            // squash to look more aerodynamic
            SquashStretch?.PlayFeedbacks();
            // whoosh sound
            _audio.PlayOneShot(dashSound);
            _timeSinceDash = 0;
        }
        _dashInput = true;
        // get pushed forward - in FixedUpdate
    }

    protected override void Interact()
    {
        if (_timeSinceAttack < delayBetweenAttacks)
        {
            return;
        }
        _timeSinceAttack = 0;
        // claws forward
        _anim.SetTrigger(attack);
        //do screech
        _audio.PlayOneShot(screechSounds[UnityEngine.Random.Range(0, screechSounds.Length)]);
    }
}