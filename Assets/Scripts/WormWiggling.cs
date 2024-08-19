using System;
using System.Collections;
using Extensions;
using UnityEngine;
using UnityEngine.Events;

public class WormWiggling : MonoBehaviour
{
    public float wiggleRate;
    public float wiggleAngle;
    public float wiggleHeight;
    public Animator cinematicAntAnimator;
    public GameObject wiggleInstructions;
    public AudioClip[] wiggleSounds;
    public AudioClip nibbleSound;

    private Transform _transChild;
    private SpriteRenderer _spriteChild;
    private Animator _anim;
    private AudioSource _audio;

    private float _totalWiggleTime = 0.000001f;
    private Vector2 _inputVector = new(0.0f, 0.0f);
    private static readonly int AnimMoving = Animator.StringToHash("moving");
    private bool _antApproaching = false;
    private bool _antReached = false;

    // I don't know how to make this WormEaten work again...
    // so I found a workaround.
    //    -- itamar
    // public UnityEvent WormEaten;
    public GameManager gameManager;

    void Awake()
    {
        _transChild = gameObject.GetComponentsInChildren<Transform>()[0];
        _spriteChild = gameObject.GetComponentInChildren<SpriteRenderer>();
        _anim = gameObject.GetComponentInChildren<Animator>();
        _audio = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        _inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
    }

    void FixedUpdate()
    {
        if (_antReached)
            return;
        var keepWiggling = _inputVector.magnitude > 0.0f || Math.Abs(_transChild.localRotation.z) < 0.995;
        if (keepWiggling)
        {
            _totalWiggleTime += Time.fixedDeltaTime;
            var cyclicalAngle = Mathf.Sin(_totalWiggleTime * wiggleRate) * wiggleAngle;
            var wigglyUp = Vector2.down.Rotate2DDeg(cyclicalAngle);
            var wigglyUpOffset = Mathf.Cos(_totalWiggleTime * wiggleRate * 2) * wiggleHeight;
            _transChild.rotation = Quaternion.LookRotation(Vector3.forward, wigglyUp);
            // _transChild.localPosition = new Vector3(0, wigglyUpOffset, 0);
            // animate movement
            _anim.SetBool(AnimMoving, true);

            if (!_audio.isPlaying)
            {
                var randomWiggleIndex = UnityEngine.Random.Range(0, wiggleSounds.Length);
                _audio.clip = wiggleSounds[randomWiggleIndex];
                _audio.Play();
            }
        }
        else
        {
            _anim.SetBool(AnimMoving, false);
            if (_audio.isPlaying)
            {
                _audio.Stop();
            }
        }

        if (_totalWiggleTime > 2)
        {
            wiggleInstructions.SetActive(false);
        }

        if (_totalWiggleTime > 3 && !_antApproaching)
        {
            _antApproaching = true;
            cinematicAntAnimator.SetTrigger("go_1");
            StartCoroutine(WaitForEndOfLevel());
        }
    }

    private IEnumerator WaitForEndOfLevel()
    {
        yield return new WaitForSeconds(2);
        _audio.Stop();
        _audio.loop = false;
        _antReached = true;
        // remove sprite renderer
        yield return new WaitForSeconds(1);
        _spriteChild.enabled = false;
        _audio.clip = nibbleSound;
        _audio.Play();
        yield return new WaitForSeconds(1);
        // WormEaten?.Invoke();
        gameManager.LoadNextScene(gameObject);
    }
}