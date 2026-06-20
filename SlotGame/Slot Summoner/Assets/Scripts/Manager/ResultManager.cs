using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI characterText;
    public TextMeshProUGUI expectationText;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI galleryMessageText;

    public Button registerButton;
    public Button titleButton;

    void Start()
    {
        resultText.text = GameData.Instance.battleResult;

        characterText.text =
            "CHARACTER：" + GameData.Instance.characterName;

        expectationText.text =
            "期待度：" + GameData.Instance.expectation + "%";

        statusText.text =
            "HP：" + GameData.Instance.playerHp + "\n" +
            "ATTACK：" + GameData.Instance.playerAttack + "\n" ;

        galleryMessageText.text = "";

        registerButton.onClick.AddListener(RegisterCharacter);
        titleButton.onClick.AddListener(GoTitle);

        if (GameData.Instance.IsUnlocked(GameData.Instance.characterName))
        {
            galleryMessageText.text = "This character is already registered.";
            registerButton.interactable = false;
        }
    }

    void RegisterCharacter()
    {
        GameData.Instance.RegisterCharacter(GameData.Instance.characterName);

        galleryMessageText.text =
            GameData.Instance.characterName + " has been registered in the gallery!";

        registerButton.interactable = false;
    }

    void GoTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}