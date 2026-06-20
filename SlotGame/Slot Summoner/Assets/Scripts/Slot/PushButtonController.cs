using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PushButtonController : MonoBehaviour
{
    public Button pushButton;
    public Image pushButtonImage;

    public Sprite normalPushSprite;
    public Sprite bluePushSprite;
    public Sprite redPushSprite;
    public Sprite goldPushSprite;
    public Sprite rainbowPushSprite;

    public Sprite swordSprite;
    public Sprite shieldSprite;
    public Sprite mageSprite;

    public TextMeshProUGUI effectText;
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI characterStatusText;

    bool pushed = false;

    void Start()
    {
        pushButton.onClick.AddListener(Push);
    }

    void OnEnable()
    {
        pushed = false;
    }

    public void UpdatePushButton()
    {
        int expectation = GameData.Instance.expectation;

        if (expectation >= 180)
        {
            pushButtonImage.sprite = rainbowPushSprite;
        }
        else if (expectation >= 120)
        {
            pushButtonImage.sprite = goldPushSprite;
        }
        else if (expectation >= 70)
        {
            pushButtonImage.sprite = redPushSprite;
        }
        else if (expectation >= 40)
        {
            pushButtonImage.sprite = bluePushSprite;
        }
        else
        {
            pushButtonImage.sprite = normalPushSprite;
        }
    }

    void Push()
    {
        if (pushed) return;

        pushed = true;

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySE(SoundManager.Instance.pushSE);
        }

        FindFirstObjectByType<SlotManager>().StopSlotReels();

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySE(SoundManager.Instance.reelStopSE);
        }

        int expectation = GameData.Instance.expectation;

        int rand = Random.Range(0, 3);

        string characterName;
        int baseHp;
        int baseAttack;
        Sprite characterSprite;

        if (rand == 0)
        {
            characterName = "Sword";
            baseHp = 120;
            baseAttack = 30;
            characterSprite = swordSprite;
        }
        else if (rand == 1)
        {
            characterName = "Shield";
            baseHp = 210;
            baseAttack = 15;
            characterSprite = shieldSprite;
        }
        else
        {
            characterName = "Mage";
            baseHp = 80;
            baseAttack = 50;
            characterSprite = mageSprite;
        }

        int finalHp = baseHp;
        int finalAttack = baseAttack;

        if (expectation >= 180)
        {
            finalHp += 180;
            finalAttack += 90;
        }
        else if (expectation >= 120)
        {
            finalHp += 120;
            finalAttack += 60;
        }
        else if (expectation >= 70)
        {
            finalHp += 70;
            finalAttack += 35;
        }
        else if (expectation >= 40)
        {
            finalHp += 40;
            finalAttack += 20;
        }
        else
        {
            finalHp += 20;
            finalAttack += 10;
        }

        GameData.Instance.characterName = characterName;
        GameData.Instance.playerHp = finalHp;
        GameData.Instance.playerAttack = finalAttack;
        GameData.Instance.characterSprite = characterSprite;

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySE(SoundManager.Instance.summonSE);
        }

        effectText.text = characterName + " ！！";

        characterNameText.text = "Character：" + characterName;

        characterStatusText.text =
            "HP：" + finalHp + "\n" +
            "ATTACK：" + finalAttack;

        pushButton.gameObject.SetActive(false);

        FindFirstObjectByType<SlotSceneManager>().ShowGoBattleButton();
    }
}