using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatusPanelController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI currentExpText;
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI defenseText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI currentGoldText;
    [SerializeField] private Image standingImage;
    [SerializeField] private CharacterImageDatabase characterImageDatabase;

    void Start()
    {
        RefreshStatus();
    }

    public void RefreshStatus()
    {
        playerNameText.text = PlayerManager.Instance.playerData.playerName;
        hpText.text = PlayerManager.Instance.playerData.currentHp.ToString() + " / " + PlayerManager.Instance.playerData.maxHp.ToString();
        levelText.text = "Lv. " + PlayerManager.Instance.playerData.level.ToString();
        currentExpText.text = PlayerManager.Instance.playerData.currentExp.ToString();
        attackText.text = PlayerManager.Instance.playerData.attack.ToString();
        defenseText.text = PlayerManager.Instance.playerData.defense.ToString();
        speedText.text = PlayerManager.Instance.playerData.speed.ToString();
        currentGoldText.text = PlayerManager.Instance.playerData.currentGold.ToString() + " G";

        RefreshStandingImage(PlayerManager.Instance.playerData.iconNumber);
    }

    private void RefreshStandingImage(int iconNumber)
    {
        if (standingImage == null || characterImageDatabase == null)
        {
            return;
        }

        CharacterImageData imageData = characterImageDatabase.GetCharacterImageData(iconNumber);

        if (imageData == null || imageData.standingImage == null)
        {
            return;
        }

        standingImage.sprite = imageData.standingImage;
        standingImage.preserveAspect = true;
    }
}