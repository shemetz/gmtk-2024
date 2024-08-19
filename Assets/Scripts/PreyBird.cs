using JetBrains.Annotations;
using UnityEngine;

public class PreyBird : MonoBehaviour
{
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float collisionDuration;
    [SerializeField] private AudioClip collisionScreech;

    private Rigidbody2D _rb;
    private AudioSource _audio;

    private float _timeSinceCollision = 999;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _audio = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        _timeSinceCollision += Time.fixedDeltaTime;
        if (_rb.velocity.x < forwardSpeed)
        {
            _rb.AddForce(new Vector2(forwardSpeed, 0));
        }

        if (_timeSinceCollision > collisionDuration)
        {
            // turn to face forward again (z-rotation 0)
            transform.rotation =
                Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.fixedDeltaTime * 3f);
        }
    }

    /** used with SendMessage */
    [UsedImplicitly]
    public void OnAccidentalCollision()
    {
        if (!_audio.isPlaying)
        {
            _timeSinceCollision = 0;
            _audio.pitch = Random.Range(0.8f, 1.2f);
            _audio.PlayOneShot(collisionScreech);
        }
    }
}