using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatusUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemyNameText;
    [SerializeField] private TextMeshProUGUI enemyLevelText;
    [SerializeField] private TextMeshProUGUI enemyHpText;
    [SerializeField] private Slider enemyHpSlider;

    public void Setup(BattleEnemyData enemy)
    {
        enemyNameText.text = enemy.enemyData.enemyName;
        enemyLevelText.text = "Lv. " + enemy.level.ToString();
        enemyHpText.text = enemy.currentHp + " / " + enemy.maxHp;
        enemyHpSlider.value = (float)enemy.currentHp / enemy.maxHp;
    }
}