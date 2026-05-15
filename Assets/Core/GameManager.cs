using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Панель поражения")]
    public GameObject gameOverPanel;

    void Awake()
    {
        // Простой синглтон
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Скрываем панель при старте
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    // Вызов поражения (этот метод будет вызывать ядро)
    public void GameOver()
    {
        Debug.Log("===== GAME OVER =====");

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
}