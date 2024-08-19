using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] MMF_Player FadeIn;
    [SerializeField] private MMF_Player GetEaten;
    [SerializeField] private Transform eatingPosition;

    private AudioSource _audio;

    private bool transitionStarted = false;

    // Start is called before the first frame update
    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    void Start()
    {
        FadeIn.PlayFeedbacks();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !transitionStarted)
        {
            transitionStarted = true;
            other.GetComponent<AnimalController>().Stop();
            LoadNextScene();
        }
    }

    public async void LoadNextScene()
    {
        if (GetEaten)
        {
            await GetEaten.PlayFeedbacksTask(eatingPosition.position);
        }

        // TODO maybe play it a bit earlier?
        _audio?.Play();
        await FadeIn.PlayFeedbacksTask(Vector3.zero, 1f, true);
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            // eaten by a t-rex, end of the game
            // (audio will play for about 7 seconds)
            // TODO go to credits after 10 seconds of darkness?
        }
    }
}