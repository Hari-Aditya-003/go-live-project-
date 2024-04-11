using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI time;
    public int score;
    public float timeRemaining = 10;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScore(0);
        UpdateTime(timeRemaining);
        gameOverText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTime(timeRemaining);

        }
        else
        {
            UpdateTime(0);
            HandleGameOver();

        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public void UpdateTime(float timeleft)
    {
        time.text = "Game Over" ;
    }

    public void HandleGameOver()
    {
        // Handle game over logic here
        gameOverText.gameObject.SetActive(true);
    }
}
  