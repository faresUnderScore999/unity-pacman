using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    public static ScoreScript Instance; // Instance statique globale

    public PlayerMovement playerMovement;
    private TextMeshProUGUI scoreText;
    private bool isGameOver = false;

    private void Awake()
    {
        // Configuration du Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (isGameOver) return;

        if (playerMovement != null && scoreText != null)
        {
            scoreText.text = "Score: " + playerMovement.score;
        }
    }

    public void ShowGameOver()
    {
        isGameOver = true;
        if (scoreText != null)
        {
            scoreText.text = "GAME OVER\nFinal Score: " + (playerMovement != null ? playerMovement.score : 0);
        }
    }
}