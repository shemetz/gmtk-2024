using Extensions;
using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WormPlayerControl : MonoBehaviour
{
    public float movementSpeed;

    private Rigidbody2D _rb;
    private Transform _trans;
    private Animator _anim;

    private Vector2 _inputVector = new Vector2(0.0f, 0.0f);
    private static readonly int AnimMoving = Animator.StringToHash("moving");

    void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _trans = gameObject.GetComponent<Transform>();
        _anim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        _inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    void FixedUpdate()
    {
        if (_inputVector.magnitude > 0.0f)
        {
            // move towards WASD
            _rb.MovePosition(_rb.position + _inputVector * (movementSpeed * Time.fixedDeltaTime));
            // rotate towards WASD
            // worm sprite is currently facing up, so we need to rotate 0 deg
            _trans.rotation = Quaternion.LookRotation(Vector3.forward, _inputVector.Rotate2DDeg(0));
            // animate movement
            _anim.SetBool(AnimMoving, true);
        }
        else
        {
            _anim.SetBool(AnimMoving, false);
        }
    }
}