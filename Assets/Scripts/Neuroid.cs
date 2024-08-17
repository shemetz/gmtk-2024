using UnityEngine;
using UnityEngine.Events;

public class Neuroid : MonoBehaviour
{
    public CommandType brainCommand;

    private Animator _anim;

    public UnityEvent<bool, CommandType> SendCommand;
    
    private static readonly int Pressed = Animator.StringToHash("pressed");

    private void Awake()
    {
        _anim = gameObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("worm"))
        {
            _anim.SetBool(Pressed, true);
            SendCommand?.Invoke(true, brainCommand);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("worm"))
        {
            _anim.SetBool(Pressed, false);
            SendCommand?.Invoke(false, brainCommand);
        }
    }
}