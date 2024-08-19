using UnityEngine;

public class PreyBird : MonoBehaviour
{
    [SerializeField] private float forwardSpeed;

    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(forwardSpeed, 0);
        // turn to face forward again (z-rotation 0)
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.fixedDeltaTime * 3f);
    }
}