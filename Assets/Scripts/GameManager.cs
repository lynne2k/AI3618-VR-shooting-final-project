using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int totalScore = 0;
    public int highestScore = 0;
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public TMP_Text timerText;
    public bool isTiming = false;

    private float gameTime = 30f; // 1 minute timer
    

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager instance created.");
        }
        else
        {
            Debug.Log("Duplicate GameManager instance destroyed.");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (isTiming)
        {
            gameTime -= Time.deltaTime;
            timerText.text = "Time Left:\t\t" + Mathf.Max(0, Mathf.RoundToInt(gameTime)) + "s";

            if (gameTime <= 0)
            {
                EndGame();
            }
        }
    }

    public void AddScore(int score)
    {
        totalScore += score;
        scoreText.text = "Current Score:\t" + totalScore;
    }

    public void StartGame()
    {
        totalScore = 0;
        gameTime = 60f;
        isTiming = true;
        scoreText.text = "Current Score:\t" + totalScore;
    }

    private void EndGame()
    {
        isTiming = false;
        if (totalScore > highestScore)
        {
            highestScore = totalScore;
            highScoreText.text = "Hi-Score:\t\t" + highestScore;
        }
    }
}
