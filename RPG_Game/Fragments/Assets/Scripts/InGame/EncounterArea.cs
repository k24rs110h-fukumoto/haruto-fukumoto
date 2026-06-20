using System.Collections.Generic;
using UnityEngine;

public class EncounterArea : MonoBehaviour
{
    [SerializeField] private int minEnemyCount = 1;
    [SerializeField] private int maxEnemyCount = 3;
    [SerializeField] private float encounterChance = 0.2f;
    [SerializeField] private float checkInterval = 1.0f;
    [SerializeField] private EnemySpawnEntry[] enemyTable;

    private bool isPlayerInside;
    private bool isEncounterStarted;
    private float checkTimer;
    private Vector3 lastPlayerPosition;
    private Transform playerTransform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        isPlayerInside = true;
        playerTransform = collision.transform;
        lastPlayerPosition = playerTransform.position;
        checkTimer = 0f;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        isPlayerInside = false;
        playerTransform = null;
        checkTimer = 0f;
    }

    private void Update()
    {
        if (isEncounterStarted) return;
        if (!isPlayerInside) return;
        if (playerTransform == null) return;

        if (!IsPlayerMoving()) return;

        checkTimer += Time.deltaTime;

        if (checkTimer >= checkInterval)
        {
            checkTimer = 0f;
            TryEncounter();
        }

        lastPlayerPosition = playerTransform.position;
    }

    private bool IsPlayerMoving()
    {
        float distance = Vector3.Distance(playerTransform.position, lastPlayerPosition);
        return distance > 0.01f;
    }

    private void TryEncounter()
    {
        float randomValue = Random.value;

        if (randomValue <= encounterChance)
        {
            StartBattle();
        }
    }

    private void StartBattle()
    {
        if (enemyTable == null || enemyTable.Length == 0)
        {
            Debug.LogWarning("Enemy Table が設定されていません。");
            return;
        }

        BattleStartManager battleStartManager = FindFirstObjectByType<BattleStartManager>();

        if (battleStartManager == null)
        {
            Debug.LogError("BattleStartManager が Scene にありません。");
            return;
        }

        int enemyCount = Random.Range(minEnemyCount, maxEnemyCount + 1);

        List<EnemyData> selectedEnemies = new List<EnemyData>();

        for (int i = 0; i < enemyCount; i++)
        {
            EnemyData enemy = GetRandomEnemy();

            if (enemy != null)
            {
                selectedEnemies.Add(enemy);
            }
        }

        if (selectedEnemies.Count == 0)
        {
            Debug.LogWarning("出現できる敵がいません。");
            return;
        }

        isEncounterStarted = true;

        battleStartManager.StartBattle(selectedEnemies.ToArray());
    }

    private EnemyData GetRandomEnemy()
    {
        int totalRate = 0;

        foreach (EnemySpawnEntry entry in enemyTable)
        {
            if (entry.enemyData == null) continue;
            if (entry.spawnRate <= 0) continue;

            totalRate += entry.spawnRate;
        }

        if (totalRate <= 0)
        {
            return null;
        }

        int randomRate = Random.Range(1, totalRate + 1);
        int currentRate = 0;

        foreach (EnemySpawnEntry entry in enemyTable)
        {
            if (entry.enemyData == null) continue;
            if (entry.spawnRate <= 0) continue;

            currentRate += entry.spawnRate;

            if (randomRate <= currentRate)
            {
                return entry.enemyData;
            }
        }

        return null;
    }
}