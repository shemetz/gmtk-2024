using UnityEngine;
using UnityEngine.Events;

public class Neuroid : MonoBehaviour
{
    public CommandType brainCommand;
    public AudioClip soundPressed;
    public AudioClip soundReleased;

    private Animator _anim;
    private AudioSource _audio;

    public UnityEvent<bool, CommandType> SendCommand;
    
    private static readonly int Pressed = Animator.StringToHash("pressed");

    private void Awake()
    {
        _anim = gameObject.GetComponent<Animator>();
        _audio = gameObject.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("worm"))
        {
            _anim.SetBool(Pressed, true);
            SendCommand?.Invoke(true, brainCommand);
            _audio.PlayOneShot(soundPressed);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("worm"))
        {
            _anim.SetBool(Pressed, false);
            SendCommand?.Invoke(false, brainCommand);
            _audio.PlayOneShot(soundReleased);
        }
    }
}