using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyData[] enemyDataList;
    [SerializeField] private BattleStartManager battleStartManager;
    [SerializeField] private bool respawn;
    [SerializeField] private string fieldEnemyID;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DisableEnemy();
            BattleDataManager.Instance.currentFieldEnemyID = fieldEnemyID;
            battleStartManager.StartBattle(enemyDataList);
        }
    }

    void Start()
    {
        if (!respawn && DefeatedEnemyManager.Instance.IsDefeated(fieldEnemyID))
        {
            DisableEnemy();
            return;
        }

        if (BattleDataManager.Instance.currentFieldEnemyID == fieldEnemyID &&
            BattleDataManager.Instance.shouldDisableFieldEnemyOnce)
        {
            DisableEnemy();
            BattleDataManager.Instance.shouldDisableFieldEnemyOnce = false;
            return;
        }

        AbleEnemy();
    }

    private void AbleEnemy()
    {
        gameObject.SetActive(true);
    }

    private void DisableEnemy()
    {
        gameObject.SetActive(false);
    }
}