using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class PreyBird : MonoBehaviour
{
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float collisionDuration;
    [SerializeField] private AudioClip collisionScreech;
    [SerializeField] private ParticleSystem blood;
    [SerializeField] private GameObject visuals;
    [field: SerializeField] private List<SpriteRenderer> renderers;
    [SerializeField] private Material greenMat, redMat;

    private Rigidbody2D _rb;
    private AudioSource _audio;

    private float _timeSinceCollision = 999;

    private void OnValidate()
    {
        foreach (var VARIABLE in GetComponentsInChildren<SpriteRenderer>())
        {
            renderers.Add(VARIABLE);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _audio = GetComponent<AudioSource>();

        if (Extensions.Extensions.GetRandomBool())
        {
            foreach (var VARIABLE in renderers)
            {
                VARIABLE.material = greenMat;
            }
        }
        else
        {
            foreach (var VARIABLE in renderers)
            {
                VARIABLE.material = redMat;
            }
        }
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

    public void Die()
    {
        blood.Play();
        visuals.SetActive(false);
        GetComponent<Collider2D>().enabled = false;
    }
}