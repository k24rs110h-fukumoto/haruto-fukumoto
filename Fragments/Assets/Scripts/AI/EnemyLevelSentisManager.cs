using UnityEngine;

public class EnemyLevelSentisManager : MonoBehaviour
{
    public static EnemyLevelSentisManager Instance;

    [Header("Temporary AI")]
    [SerializeField] private bool useTemporaryAI = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public int PredictLevelAdjustment(float[] features)
    {
        if (features == null || features.Length < 10)
        {
            return 0;
        }

        if (useTemporaryAI)
        {
            return PredictByTemporaryAI(features);
        }

        // 後でここをSentis推論に差し替える
        return PredictByTemporaryAI(features);
    }

    private int PredictByTemporaryAI(float[] features)
    {
        float winRate = features[0];
        float loseRate = features[1];
        float escapeRate = features[2];
        float averagePlayerDamage = features[3];
        float averageEnemyDamage = features[4];
        float magicUseRate = features[5];
        float itemUseRate = features[6];
        float healRate = features[7];
        float criticalRate = features[8];
        float missRate = features[9];

        float score = 0f;

        score += winRate * 2f;
        score -= loseRate * 2f;
        score -= escapeRate * 1f;

        if (averagePlayerDamage > averageEnemyDamage * 1.5f)
        {
            score += 1f;
        }

        if (averageEnemyDamage > averagePlayerDamage * 1.5f)
        {
            score -= 1f;
        }

        if (magicUseRate > 0.5f)
        {
            score += 0.5f;
        }

        if (healRate > 0.4f)
        {
            score += 0.5f;
        }

        if (missRate > 0.25f)
        {
            score -= 0.5f;
        }

        if (criticalRate > 0.25f)
        {
            score += 0.5f;
        }

        int adjustment = Mathf.RoundToInt(score);
        adjustment = Mathf.Clamp(adjustment, -2, 3);

        return adjustment;
    }
}