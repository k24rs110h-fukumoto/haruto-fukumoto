using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public GameObject enemySelectPanel;
    public GameObject battlePanel;

    public Button pandaButton;
    public Button draculaButton;
    public Button angelButton;
    public Button demonKingButton;

    public Image playerImage;
    public Image enemyImage;

    public Sprite pandaSprite;
    public Sprite draculaSprite;
    public Sprite angelSprite;
    public Sprite demonKingSprite;

    public TextMeshProUGUI playerStatusText;
    public TextMeshProUGUI enemyNameText;
    public TextMeshProUGUI enemyStatusText;
    public TextMeshProUGUI battleLogText;

    public Button attackButton;

    int playerHp;
    int playerAttack;

    string enemyName;
    int enemyHp;
    int enemyAttack;

    void Start()
    {
        enemySelectPanel.SetActive(true);
        battlePanel.SetActive(false);

        pandaButton.onClick.AddListener(() => SelectEnemy("Panda", 100, 15, pandaSprite));
        draculaButton.onClick.AddListener(() => SelectEnemy("Dracula", 160, 25, draculaSprite));
        angelButton.onClick.AddListener(() => SelectEnemy("Angel", 220, 35, angelSprite));
        demonKingButton.onClick.AddListener(() => SelectEnemy("Demon King", 500, 45, demonKingSprite));

        attackButton.onClick.AddListener(PlayerAttack);
    }

    void SelectEnemy(string name, int hp, int attack, Sprite sprite)
    {
        enemyName = name;
        enemyHp = hp;
        enemyAttack = attack;

        playerHp = GameData.Instance.playerHp;
        playerAttack = GameData.Instance.playerAttack;

        playerImage.sprite = GameData.Instance.characterSprite;
        enemyImage.sprite = sprite;

        enemySelectPanel.SetActive(false);
        battlePanel.SetActive(true);

        UpdateUI();

        battleLogText.text = GameData.Instance.characterName + " VS " + enemyName;
    }

    void PlayerAttack()
    {
        enemyHp -= playerAttack;
        if (enemyHp < 0) enemyHp = 0;

        battleLogText.text =
            GameData.Instance.characterName + "is attacking！\n" +
            playerAttack + "damage！";

        if (enemyHp <= 0)
        {
            Win();
            return;
        }

        playerHp -= enemyAttack;
        if (playerHp < 0) playerHp = 0;

        battleLogText.text +=
            "\n" + enemyName + "is attacking！\n" +
            enemyAttack + "damage！";

        if (playerHp <= 0)
        {
            Lose();
            return;
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        playerStatusText.text =
            "CHARACTER：" + GameData.Instance.characterName + "\n" +
            "HP：" + playerHp + "\n" +
            "ATTACK：" + playerAttack;

        enemyNameText.text = "ENEMY：" + enemyName;

        enemyStatusText.text =
            "HP：" + enemyHp + "\n" +
            "ATTACK：" + enemyAttack;
    }

    void Win()
    {
        GameData.Instance.battleResult = "VICTORY";
        SceneManager.LoadScene("ResultScene");
    }

    void Lose()
    {
        GameData.Instance.battleResult = "DEFEAT";
        SceneManager.LoadScene("ResultScene");
    }
}