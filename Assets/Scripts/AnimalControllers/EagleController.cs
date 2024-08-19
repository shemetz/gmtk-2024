using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using GMTK.PlatformerToolkit;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class EagleController : AnimalController
{
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float diveSpeed;
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float rotationAngle;
    [SerializeField] private float minimumDashDuration;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float delayBetweenAttacks;
    [SerializeField] private float attackDuration;
    [SerializeField] private float bumpEffectDuration;
    [SerializeField] private MMF_Player SquashStretch;
    [SerializeField] private AudioClip[] screechSounds;
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private AudioClip bumpSound;

    private AudioSource _audio;
    private Animator _anim;
    private Rigidbody2D _rb;

    private Vector2 _moveInput;
    private float _timeSinceAttack = 999;
    private float _timeSinceDash = 999;
    private float _timeSinceBump = 999;
    private bool _dashInput = false;

    private readonly int _isAttacking = Animator.StringToHash("isAttacking");
    private readonly int _isDashing = Animator.StringToHash("isDashing");

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

        if (_timeSinceBump > bumpEffectDuration)
        {
            float horizontalSpeed;
            if (_dashInput || _timeSinceDash < minimumDashDuration)
                horizontalSpeed = dashSpeed;
            else if (_timeSinceAttack < attackDuration)
                horizontalSpeed = (dashSpeed + forwardSpeed) / 2;
            else horizontalSpeed = forwardSpeed;
            if (_moveInput.y > 0)
                // slower when ascending
                horizontalSpeed = forwardSpeed * 0.5f;
            if (_moveInput.y < 0)
                // faster when diving
                horizontalSpeed = diveSpeed;
            _rb.velocity = new Vector2(horizontalSpeed, verticalSpeed * _moveInput.y);

            // rotate a bit up or down or reset, depending on vertical move input
            // lerping slowly to avoid sudden changes
            float targetRotation = _moveInput.y * rotationAngle;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, targetRotation),
                Time.fixedDeltaTime * 20f);
        }

        if (_timeSinceAttack > attackDuration)
        {
            _anim.SetBool(_isAttacking, false);
        }

        if (_timeSinceDash > minimumDashDuration && !_dashInput)
        {
            _anim.SetBool(_isDashing, false);
        }

        _timeSinceAttack += Time.fixedDeltaTime;
        _timeSinceDash += Time.fixedDeltaTime;
        _timeSinceBump += Time.fixedDeltaTime;
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
            _anim.SetBool(_isDashing, true);
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
        _anim.SetBool(_isAttacking, true);
        //do screech
        _audio.PlayOneShot(screechSounds[UnityEngine.Random.Range(0, screechSounds.Length)]);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PreyBird"))
        {
            // dive from above = if collision contact is within this area
            //
            //  \  |  /      "+135 deg"
            //   \ | /
            //    \|/
            // ----X (prey bird)
            //    /| 
            //   / |
            //  /  |       "-90 deg"  (= eagle beak touches bird butt.  I'm counting it as a dive attack)
            var collisionNormal = collision.GetContact(0).normal;
            var angleFromLeft = Vector2.SignedAngle(Vector2.left, collisionNormal);
            // reminder:  angle is counterclockwise
            var isDiveCollisionFromAbove = _moveInput.y < 0 && angleFromLeft is < 135 and > -90;

            var isOffensive = isDiveCollisionFromAbove || _timeSinceAttack < attackDuration;
            if (isOffensive && _timeSinceBump > bumpEffectDuration)
            {
                // this kills the prey bird
                collision.gameObject.GetComponent<PreyBird>().Die();
                // TODO some cool sound effect
                // TODO some cool visual effect
            }
            else
            {
                // eagle bumps into bird, gets stuck in place for a moment
                _timeSinceBump = 0;
                // stop any current dash
                _timeSinceDash = 999;
                _anim.SetBool(_isDashing, false);
                // stop forward momentum (keeping backward and vertical)
                _rb.velocity = new Vector2(Math.Min(_rb.velocity.x, 0), _rb.velocity.y);
                // play bump sound
                _audio.PlayOneShot(bumpSound);
                collision.gameObject.SendMessage("OnAccidentalCollision");
            }
        }
    }
}