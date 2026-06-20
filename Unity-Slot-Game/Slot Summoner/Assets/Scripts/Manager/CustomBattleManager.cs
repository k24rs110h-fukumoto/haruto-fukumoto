using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CustomBattleManager : MonoBehaviour
{
    public GameObject characterSelectPanel;
    public GameObject enemySelectPanel;
    public GameObject battlePanel;

    public Button swordButton;
    public Button shieldButton;
    public Button mageButton;

    public Button pandaButton;
    public Button draculaButton;
    public Button angelButton;
    public Button demonKingButton;

    public TextMeshProUGUI playerStatusText;
    public TextMeshProUGUI enemyNameText;
    public TextMeshProUGUI enemyStatusText;
    public TextMeshProUGUI battleLogText;

    public Button attackButton;
    public Button titleButton;

    string playerName;
    int playerHp;
    int playerAttack;
    string skillName;

    string enemyName;
    int enemyHp;
    int enemyAttack;

    void Start()
    {
        characterSelectPanel.SetActive(true);
        enemySelectPanel.SetActive(false);
        battlePanel.SetActive(false);

        swordButton.interactable = GameData.Instance.IsUnlocked("剣士");
        shieldButton.interactable = GameData.Instance.IsUnlocked("盾");
        mageButton.interactable = GameData.Instance.IsUnlocked("魔導士");

        swordButton.onClick.AddListener(() => SelectCharacter("剣士", 120, 30, "ブレイズスラッシュ"));
        shieldButton.onClick.AddListener(() => SelectCharacter("盾", 210, 15, "パーフェクトガード"));
        mageButton.onClick.AddListener(() => SelectCharacter("魔導士", 80, 50, "メテオ"));

        pandaButton.onClick.AddListener(() => SelectEnemy("パンダ", 100, 15));
        draculaButton.onClick.AddListener(() => SelectEnemy("ドラキュラ", 160, 25));
        angelButton.onClick.AddListener(() => SelectEnemy("エンジェル", 220, 35));
        demonKingButton.onClick.AddListener(() => SelectEnemy("魔王", 500, 45));

        attackButton.onClick.AddListener(PlayerAttack);
        titleButton.onClick.AddListener(() => SceneManager.LoadScene("TitleScene"));
    }

    void SelectCharacter(string name, int hp, int attack, string skill)
    {
        playerName = name;
        playerHp = hp;
        playerAttack = attack;
        skillName = skill;

        characterSelectPanel.SetActive(false);
        enemySelectPanel.SetActive(true);
    }

    void SelectEnemy(string name, int hp, int attack)
    {
        enemyName = name;
        enemyHp = hp;
        enemyAttack = attack;

        enemySelectPanel.SetActive(false);
        battlePanel.SetActive(true);

        UpdateUI();

        battleLogText.text = playerName + " VS " + enemyName;
    }

    void PlayerAttack()
    {
        enemyHp -= playerAttack;

        if (enemyHp < 0)
        {
            enemyHp = 0;
        }

        battleLogText.text =
            playerName + "の攻撃！\n" +
            playerAttack + "ダメージ！";

        if (enemyHp <= 0)
        {
            battleLogText.text += "\n勝利！！";
            attackButton.gameObject.SetActive(false);
            return;
        }

        playerHp -= enemyAttack;

        if (playerHp < 0)
        {
            playerHp = 0;
        }

        battleLogText.text +=
            "\n" + enemyName + "の攻撃！\n" +
            enemyAttack + "ダメージ！";

        if (playerHp <= 0)
        {
            battleLogText.text += "\n敗北…";
            attackButton.gameObject.SetActive(false);
            return;
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        playerStatusText.text =
            "キャラ：" + playerName + "\n" +
            "HP：" + playerHp + "\n" +
            "攻撃：" + playerAttack + "\n" +
            "必殺技：" + skillName;

        enemyNameText.text = "敵：" + enemyName;

        enemyStatusText.text =
            "HP：" + enemyHp + "\n" +
            "攻撃：" + enemyAttack;
    }
}