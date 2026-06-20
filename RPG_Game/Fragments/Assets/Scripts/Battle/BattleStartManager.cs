using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleStartManager : MonoBehaviour
{
    [SerializeField] private string battleSceneName;

    public void StartBattle(EnemyData[] enemies)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player == null)
        {
            return;
        }
        BattleDataManager.Instance.currentEnemyDataList = enemies;
        string returnSceneName = SceneManager.GetActiveScene().name;
        Vector3 playerPos = player.transform.position;
        BattleDataManager.Instance.returnPlayerPosition = playerPos;
        BattleDataManager.Instance.returnSceneName = returnSceneName;
        GameStateManager.SetState(GameStateManager.GameState.Battle);
        SceneTransitionManager.LoadScene(battleSceneName, "");
    }
}