using Extensions;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WormPlayerControl : MonoBehaviour
{
    public float movementSpeed;

    private Rigidbody2D _rb;
    private Transform _trans;

    private Vector2 _inputVector = new Vector2(0.0f, 0.0f);

    void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _trans = gameObject.GetComponent<Transform>();
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
            // worm sprite is currently facing left, so we need to rotate -90 deg
            _trans.rotation = Quaternion.LookRotation(Vector3.forward, _inputVector.Rotate2DDeg(-90));
        }
    }
}