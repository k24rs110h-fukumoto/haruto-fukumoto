using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AppleCatchGameManager : MonoBehaviour
{
    [Header("Game Setting")]
    [SerializeField] private string gameId = "apple_reflex_game_001";
    [SerializeField] private float timeLimit = 30f;

    [Header("Apple Spawn Setting")]
    [SerializeField] private GameObject applePrefab;
    [SerializeField] private float minSpawnInterval = 0.4f;
    [SerializeField] private float maxSpawnInterval = 1.2f;
    [SerializeField] private Transform leftSpawnPoint;
    [SerializeField] private Transform rightSpawnPoint;
    [SerializeField] private float spawnY = 5f;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI messageText;

    [Header("UI Button")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private AudioClip bgmClip;

    private int score;
    private float remainingTime;
    private float spawnTimer;
    private float nextSpawnTime;
    private bool isPlaying;

    private void Start()
    {
        ResetGame();
    }

    private void Update()
    {
        if (!isPlaying)
        {
            return;
        }

        remainingTime -= Time.deltaTime;
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= nextSpawnTime)
        {
            SpawnApple();
            spawnTimer = 0f;
            SetNextSpawnTime();
        }

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            EndGame();
        }

        UpdateUI();
    }

    public void StartGame()
    {
        score = 0;
        remainingTime = timeLimit;
        spawnTimer = 0f;
        isPlaying = true;

        SetNextSpawnTime();

        if (messageText != null)
        {
            messageText.text = "りんごをタップ！";
        }

        if (startButton != null)
        {
            startButton.gameObject.SetActive(false);
        }

        if (resetButton != null)
        {
            resetButton.gameObject.SetActive(false);
        }

        AudioManager.Instance.PlayBGM(bgmClip);
        UpdateUI();
    }

    public void AddScore()
    {
        if (!isPlaying)
        {
            return;
        }

        score++;
        UpdateUI();
    }

    private void SpawnApple()
    {
        if (applePrefab == null)
        {
            return;
        }

        float randomX = Random.Range(leftSpawnPoint.position.x, rightSpawnPoint.position.x);
        Vector3 spawnPosition = new Vector3(randomX, spawnY, 0f);

        GameObject apple = Instantiate(applePrefab, spawnPosition, Quaternion.identity);

        AppleController appleController = apple.GetComponent<AppleController>();

        if (appleController != null)
        {
            appleController.SetGameManager(this);
        }
    }

    private void SetNextSpawnTime()
    {
        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }

    public void EndGame()
    {
        if (!isPlaying)
        {
            return;
        }

        isPlaying = false;
        DeleteAllApples();

        ResultData result = new ResultData
        {
            gameId = gameId,
            score = score,
            clear = true,
            playTime = timeLimit
        };

        if (messageText != null)
        {
            messageText.text =
                "終了！\n" +
                "取ったりんご：" + result.score + "個";
        }

        if (resetButton != null)
        {
            resetButton.gameObject.SetActive(true);
        }

        Debug.Log(JsonUtility.ToJson(result));
        AudioManager.Instance.StopBGM();
        UpdateUI();
    }

    public void ResetGame()
    {
        score = 0;
        remainingTime = timeLimit;
        spawnTimer = 0f;
        isPlaying = false;

        DeleteAllApples();

        if (titleText != null)
        {
            titleText.text = "Apple Reflex Game";
        }

        if (messageText != null)
        {
            messageText.text = "Startを押して開始";
        }

        if (startButton != null)
        {
            startButton.gameObject.SetActive(true);
        }

        if (resetButton != null)
        {
            resetButton.gameObject.SetActive(false);
        }

        AudioManager.Instance.StopBGM();
        UpdateUI();
    }

    private void DeleteAllApples()
    {
        AppleController[] apples = FindObjectsByType<AppleController>(FindObjectsSortMode.None);

        foreach (AppleController apple in apples)
        {
            Destroy(apple.gameObject);
        }
    }

    private void UpdateUI()
    {
        if (timeText != null)
        {
            timeText.text = "Time: " + remainingTime.ToString("F1");
        }

        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}