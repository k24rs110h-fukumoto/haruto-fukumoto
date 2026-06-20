using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    public static BattleUIManager Instance;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI playerLevelText;
    [SerializeField] private TextMeshProUGUI playerHpText;
    [SerializeField] private Slider playerHpSlider;
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject enemyStatusPrefab;

    void Awake()
    {
        Instance = this;
    }

    public void RefreshUI()
    {
        playerNameText.text = PlayerManager.Instance.playerData.playerName;
        playerLevelText.text = "Lv. " + PlayerManager.Instance.playerData.level.ToString();
        playerHpText.text = BattleManager.Instance.playerBattleData.currentHp + " / " + BattleManager.Instance.playerBattleData.maxHp;
        playerHpSlider.value = (float)BattleManager.Instance.playerBattleData.currentHp / BattleManager.Instance.playerBattleData.maxHp;

        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
        foreach (BattleEnemyData enemy in BattleManager.Instance.battleEnemyDataList)
        {
            GameObject obj = Instantiate(enemyStatusPrefab, contentParent);
            EnemyStatusUI enemyUI = obj.GetComponent<EnemyStatusUI>();
            enemyUI.Setup(enemy);
        }
    }
}