using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    public int expectation;
    public string characterName;
    public int playerHp;
    public int playerAttack;
    public Sprite characterSprite;
    public string battleResult;

    public List<string> effectLogs = new List<string>();
    public List<string> unlockedCharacters = new List<string>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadGallery();
    }

    public void ResetSlotData()
    {
        expectation = 0;
        characterName = "";
        playerHp = 0;
        playerAttack = 0;
        characterSprite = null;
        battleResult = "";
        effectLogs.Clear();
    }

    public void AddExpectation(int value)
    {
        expectation += value;
    }

    public void AddEffectLog(string effectName)
    {
        effectLogs.Add(effectName);
    }

    public void RegisterCharacter(string name)
    {
        if (!unlockedCharacters.Contains(name))
        {
            unlockedCharacters.Add(name);
            SaveGallery();
        }
    }

    public bool IsUnlocked(string name)
    {
        return unlockedCharacters.Contains(name);
    }

    void SaveGallery()
    {
        PlayerPrefs.SetString("GalleryCharacters", string.Join(",", unlockedCharacters));
        PlayerPrefs.Save();
    }

    void LoadGallery()
    {
        unlockedCharacters.Clear();

        string data = PlayerPrefs.GetString("GalleryCharacters", "");
        if (string.IsNullOrEmpty(data)) return;

        string[] names = data.Split(',');

        foreach (string name in names)
        {
            if (!string.IsNullOrEmpty(name))
            {
                unlockedCharacters.Add(name);
            }
        }
    }
}