using TMPro;
using UnityEngine;

public class EnemySelectUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemyNameText;
    [SerializeField] private TextMeshProUGUI enemyHpText;
    private string enemyName;

    public void Setup(BattleEnemyData enemy)
    {
        enemyName = enemy.enemyData.enemyName;
        enemyNameText.text = enemyName;
        enemyHpText.text = enemy.currentHp + " / " + enemy.maxHp;
    }

    public void SetSelected(bool isSelected)
    {
        if (isSelected)
        {
            enemyNameText.text = "＞ " + enemyName;
        }
        else
        {
            enemyNameText.text = enemyName;
        }
    }
}