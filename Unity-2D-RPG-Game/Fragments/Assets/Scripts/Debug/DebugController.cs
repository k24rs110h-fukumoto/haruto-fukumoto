using UnityEngine;
using UnityEngine.UI;

public class DebugController : MonoBehaviour
{
    [SerializeField] private Button titleButton;
    [SerializeField] private Button area01_Town;
    [SerializeField] private Button area01_Field;
    [SerializeField] private Button battleButton;

    [Header("Debug Buttons")]
    [SerializeField] private Button allItemButton;
    [SerializeField] private Button allMagicButton;

    void Start()
    {
        SoundManager.Instance.PlayBGM(BGMType.Title);

        if (titleButton != null)
        {
            titleButton.onClick.AddListener(OnTitleClick);
        }

        if (area01_Town != null)
        {
            area01_Town.onClick.AddListener(OnArea01TownClick);
        }

        if (area01_Field != null)
        {
            area01_Field.onClick.AddListener(OnArea01FieldClick);
        }

        if (battleButton != null)
        {
            battleButton.onClick.AddListener(OnBattleClick);
        }

        if (allItemButton != null)
        {
            allItemButton.onClick.AddListener(GetAllItems);
        }

        if (allMagicButton != null)
        {
            allMagicButton.onClick.AddListener(GetAllMagics);
        }
    }

    private void OnTitleClick()
    {
        SoundManager.Instance.PlaySE(SEType.Button);
        SceneTransitionManager.LoadScene("TitleScene", "");
    }

    private void OnArea01TownClick()
    {
        SoundManager.Instance.PlaySE(SEType.Button);
        SoundManager.Instance.PlayBGM(BGMType.FirstTown);
        SceneTransitionManager.LoadScene("Area01_Town", "Area01_Town_Start");
    }

    private void OnArea01FieldClick()
    {
        SoundManager.Instance.PlaySE(SEType.Button);
        SceneTransitionManager.LoadScene("Area01_Field", "Area01Field_FromArea01Town");
    }

    private void OnBattleClick()
    {
        SoundManager.Instance.PlaySE(SEType.Button);
        GameStateManager.SetState(GameStateManager.GameState.Battle);
        SceneTransitionManager.LoadScene("BattleScene", "");
    }

    private void GetAllItems()
    {
        SoundManager.Instance.PlaySE(SEType.Button);

        ItemData[] items = Resources.LoadAll<ItemData>("Item");

        foreach (ItemData item in items)
        {
            InventoryManager.Instance.AddItem(item, 99);
        }

        Debug.Log("全アイテムを取得しました。取得数: " + items.Length);
    }

    private void GetAllMagics()
    {
        SoundManager.Instance.PlaySE(SEType.Button);

        MagicData[] magics = Resources.LoadAll<MagicData>("Magic");

        foreach (MagicData magic in magics)
        {
            MagicManager.Instance.AddMagic(magic, magic.maxUses);
        }

        Debug.Log("全魔法を取得しました。取得数: " + magics.Length);
    }

    void OnDestroy()
    {
        if (titleButton != null)
        {
            titleButton.onClick.RemoveListener(OnTitleClick);
        }

        if (area01_Town != null)
        {
            area01_Town.onClick.RemoveListener(OnArea01TownClick);
        }

        if (area01_Field != null)
        {
            area01_Field.onClick.RemoveListener(OnArea01FieldClick);
        }

        if (battleButton != null)
        {
            battleButton.onClick.RemoveListener(OnBattleClick);
        }

        if (allItemButton != null)
        {
            allItemButton.onClick.RemoveListener(GetAllItems);
        }

        if (allMagicButton != null)
        {
            allMagicButton.onClick.RemoveListener(GetAllMagics);
        }
    }
}