using System;
using Extensions;
using UnityEditor.Animations;
using UnityEngine;

public class WormWiggling : MonoBehaviour
{
    public float wiggleRate;
    public float wiggleAngle;
    public float wiggleHeight;
    public Animator cinematicAntAnimator;

    private Transform _transChild;
    private Animator _anim;

    private float _totalWiggleTime = 0.000001f;
    private Vector2 _inputVector = new(0.0f, 0.0f);
    private static readonly int AnimMoving = Animator.StringToHash("moving");

    void Awake()
    {
        _transChild = gameObject.GetComponentsInChildren<Transform>()[1];
        _anim = gameObject.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        _inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    void FixedUpdate()
    {
        var keepWiggling = _inputVector.magnitude > 0.0f || Math.Abs(_transChild.localRotation.z) > 0.02;
        if (keepWiggling)
        {
            _totalWiggleTime += Time.deltaTime;
            var cyclicalAngle = Mathf.Sin(_totalWiggleTime * wiggleRate) * wiggleAngle;
            var wigglyUp = Vector2.up.Rotate2DDeg(cyclicalAngle);
            var wigglyUpOffset = Mathf.Cos(_totalWiggleTime * wiggleRate * 2) * wiggleHeight;
            _transChild.rotation = Quaternion.LookRotation(Vector3.forward, wigglyUp);
            _transChild.localPosition = new Vector3(0, wigglyUpOffset, 0);
            // animate movement
            _anim.SetBool(AnimMoving, true);
        }
        else
        {
            _anim.SetBool(AnimMoving, false);
        }

        if (_totalWiggleTime > 5)
        {
            cinematicAntAnimator.SetTrigger("go_1");
        }
    }
}