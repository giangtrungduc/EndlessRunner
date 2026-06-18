using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject gamePanel;
    public TextMeshProUGUI titleText;
    public string playString;
    public string overString;

    [Header("Score")]
    public TextMeshProUGUI scoreText;
    [SerializeField] private float score = 0f;
    [SerializeField] private float baseScoreRate = 10f;
    [SerializeField] private float scoreAcceleration = 0.05f;
    
    [Header("Speed")]
    [SerializeField] private float gameSpeed = 0f;
    private float currentGameSpeed;
    [SerializeField] private float maxGameSpeed = 15f;
    [SerializeField] private float speedIncreaseRate = 0.05f;

    [Header("GamePlay")]
    public GameObject player;
    public GameObject spawnObstacle;

    private bool isGameOver;
    public float GameSpeed => currentGameSpeed;
    public int Score => Mathf.FloorToInt(score);
    private void Awake()
    {
        Instance = this;
        currentGameSpeed = gameSpeed;
    }
    private void Start()
    {
        StartGame();
    }
    private void Update()
    {
        if(gamePanel.activeSelf && IsScreenPressed())
        {
            PlayGame();
        }
        if(isGameOver) return;

        UpdateScore();
        UpdateGameSpeed();
    }
    private void UpdateScore()
    {
        float currentScoreRate = baseScoreRate + score * scoreAcceleration;
        score += currentScoreRate * Time.deltaTime;
        if(scoreText != null)
        {
            scoreText.text = "" + Score;
        }
    }
    private void UpdateGameSpeed()
    {
        currentGameSpeed += speedIncreaseRate * Time.deltaTime;
        currentGameSpeed = Mathf.Clamp(gameSpeed, 0f, maxGameSpeed);
    }
    public void StartGame()
    {
        isGameOver = true;
        player.SetActive(false);
        currentGameSpeed = 0;
        gamePanel.SetActive(true);
        titleText.text = playString;
    }
    public void PlayGame()
    {
        isGameOver = false;
        player.transform.position = Vector3.zero;
        player.SetActive(true);
        score = 0;
        currentGameSpeed = gameSpeed;
        gamePanel.SetActive(false);
    }
    public void GameOver()
    {
        isGameOver = true;
        player.SetActive(false);
        currentGameSpeed = 0;
        gamePanel.SetActive(true);
        titleText.text = overString;
        if(spawnObstacle != null)
        {
            spawnObstacle.GetComponent<SpawnObstacle>().ResetSpawner();
        }
    }
    private bool IsScreenPressed()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                return true;
            }
        }

        return false;
    }
}
