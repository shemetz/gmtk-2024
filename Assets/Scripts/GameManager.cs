using System.Collections;
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
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            FadeIn.PlayFeedbacks();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !transitionStarted)
        {
            transitionStarted = true;
            collision.GetComponent<AnimalController>().Stop();
            LoadNextScene(collision.gameObject);
        }
    }

    public async void LoadNextScene(GameObject animal)
    {
        if (GetEaten)
        {
            await GetEaten.PlayFeedbacksTask(eatingPosition.position);
        }

        // TODO maybe play it a bit earlier?
        if (_audio)
        {
            _audio?.Play();
        }
        await FadeIn.PlayFeedbacksTask(Vector3.zero, 1f, true);
        if (SceneManager.GetActiveScene().name != "EagleScene")
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else // Scene 4:  eagle ending
        {
            // eaten by a t-rex, end of the game
            animal.SetActive(false);
            // (audio will play for about 7 seconds)
            StartCoroutine(WaitAndExitGame());
        }
    }

    IEnumerator WaitAndExitGame()
    {
        yield return new WaitForSeconds(9);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Credits()
    {
        SceneManager.LoadScene(5);
    }
}