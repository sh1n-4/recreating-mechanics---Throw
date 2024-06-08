using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    private int score = 0;

    private void OnEnable()
    {
        Debug.Log("ScoreManager enabled, subscribing to OnTargetDestroyed event.");
        TargetDestroyedEvent.OnTargetDestroyed += IncrementScore;
    }

    private void OnDisable()
    {
        Debug.Log("ScoreManager disabled, unsubscribing from OnTargetDestroyed event.");
        TargetDestroyedEvent.OnTargetDestroyed -= IncrementScore;
    }

    private void IncrementScore()
    {
        score++;
        Debug.Log("Score incremented. New score: " + score);
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            Debug.Log("Updating score UI. New score text: " + "Score: " + score.ToString());
            scoreText.text = "Score: " + score.ToString();
        }
    }
}
