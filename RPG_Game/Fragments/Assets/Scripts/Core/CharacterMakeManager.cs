using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMakeManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button decideButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button[] characterButtons;
    [SerializeField] private Image[] selectedFrames;
    [SerializeField] private string startSceneName = "Area01_Field";

    private int selectedIconNumber;

    private void Start()
    {
        if (decideButton != null)
        {
            decideButton.onClick.AddListener(OnDecideButton);
        }

        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButton);
        }

        for (int i = 0; i < characterButtons.Length; i++)
        {
            int index = i;

            if (characterButtons[index] != null)
            {
                characterButtons[index].onClick.AddListener(() =>
                {
                    SelectCharacter(index);
                });
            }
        }

        selectedIconNumber = 0;
        UpdateSelectedFrame();
    }

    private void SelectCharacter(int iconNumber)
    {
        selectedIconNumber = iconNumber;
        UpdateSelectedFrame();
    }

    private void UpdateSelectedFrame()
    {
        for (int i = 0; i < selectedFrames.Length; i++)
        {
            if (selectedFrames[i] != null)
            {
                selectedFrames[i].gameObject.SetActive(i == selectedIconNumber);
            }
        }
    }

    private void OnDecideButton()
    {
        if (PlayerManager.Instance == null)
        {
            Debug.LogError("PlayerManagerがありません。");
            return;
        }

        if (PlayerManager.Instance.playerData == null)
        {
            Debug.LogError("PlayerDataがありません。");
            return;
        }

        CreateInitialPlayerData();

        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.SaveGame(1);
        }

        SceneTransitionManager.LoadScene(startSceneName, "");
    }

    private void CreateInitialPlayerData()
    {
        PlayerData playerData = PlayerManager.Instance.playerData;

        if (string.IsNullOrWhiteSpace(nameInputField.text))
        {
            playerData.playerName = "名無し";
        }
        else
        {
            playerData.playerName = nameInputField.text;
        }

        playerData.iconNumber = selectedIconNumber;

        // 初期ステータス

        playerData.level = 1;

        playerData.currentExp = 0;
        playerData.currentGold = 0;

        playerData.maxHp = 30;
        playerData.currentHp = 30;

        playerData.attack = 10;
        playerData.defense = 5;
        playerData.speed = 5;

        Debug.Log("キャラクター作成完了");

        Debug.Log("名前 : " + playerData.playerName);
        Debug.Log("アイコン : " + playerData.iconNumber);
    }

    private void OnBackButton()
    {
        SceneTransitionManager.LoadScene("TitleScene", "");
    }
}