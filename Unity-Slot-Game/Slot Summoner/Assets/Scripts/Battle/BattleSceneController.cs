using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleSceneController : MonoBehaviour
{
    public Image playerImage;
    public Image enemyImage;

    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI enemyNameText;
    public TextMeshProUGUI battleLogText;

    public CharacterData selectedPlayer;
    public CharacterData selectedEnemy;

    void Start()
    {
        SetupBattle();
    }

    void SetupBattle()
    {
        playerImage.sprite = selectedPlayer.characterImage;
        enemyImage.sprite = selectedEnemy.characterImage;

        playerNameText.text = selectedPlayer.characterName;
        enemyNameText.text = selectedEnemy.characterName;

        battleLogText.text = selectedEnemy.characterName + " が現れた！";
    }
}