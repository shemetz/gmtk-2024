using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] MMF_Player FadeIn;

    private bool transitionStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        FadeIn.PlayFeedbacks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !transitionStarted)
        {
            transitionStarted = true;
            StartCoroutine(LoadNextScene());
        }
    }

    private IEnumerator LoadNextScene()
    {
        FadeIn.PlayFeedbacksInReverse();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
