using System.Collections.Generic;
using UnityEngine;

public class DefeatedEnemyManager : MonoBehaviour
{
    public static DefeatedEnemyManager Instance;
    private HashSet<string> defeatedEnemies;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        defeatedEnemies = new HashSet<string>();
    }

    public void AddDefeatedEnemy(string enemyID)
    {
        defeatedEnemies.Add(enemyID);
    }

    public void RemoveDefeatedEnemy(string enemyID)
    {
        defeatedEnemies.Remove(enemyID);
    }

    public bool IsDefeated(string enemyID)
    {
        return defeatedEnemies.Contains(enemyID);
    }
}