using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GalleryManager : MonoBehaviour
{
    public TextMeshProUGUI galleryText;
    public Button titleButton;

    void Start()
    {
        UpdateGallery();

        titleButton.onClick.AddListener(GoTitle);
    }

    void UpdateGallery()
    {
        string text = "キャラクター図鑑\n\n";

        text += GetCharacterLine("剣士", "バランス型", 120, 30, "ブレイズスラッシュ");
        text += GetCharacterLine("盾", "高耐久型", 210, 15, "パーフェクトガード");
        text += GetCharacterLine("魔導士", "高火力型", 80, 50, "メテオ");

        galleryText.text = text;
    }

    string GetCharacterLine(string name, string type, int hp, int attack, string skill)
    {
        if (GameData.Instance.IsUnlocked(name))
        {
            return
                "【登録済み】" + name + "\n" +
                "タイプ：" + type + "\n" +
                "HP：" + hp + "\n" +
                "攻撃：" + attack + "\n" ;
        }

        return
            "【未登録】？？？\n" +
            "タイプ：？？？\n" +
            "HP：？？？\n" +
            "攻撃：？？？\n" ;
    }

    void GoTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}