using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [SerializeField]
    private QuestData[] questList;
    private Dictionary<string, QuestData> questDictionary;
    private Dictionary<string, QuestSaveData> questSaveDictionary;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        questDictionary = new Dictionary<string, QuestData>();
        questSaveDictionary = new Dictionary<string, QuestSaveData>();

        RegisterQuestData();
    }

    // クエスト登録処理（一回のみ）
    private void RegisterQuestData()
    {
        foreach (QuestData quest in questList)
        {
            questDictionary.Add(quest.questID, quest);
            QuestSaveData saveData = new QuestSaveData();
            saveData.questID = quest.questID;
            saveData.state = QuestState.NotStarted;
            saveData.currentProgress = 0;
            questSaveDictionary.Add(quest.questID, saveData);
        }
    }

    // クエスト開始処理
    public void AcceptQuest(string questID)
    {
        if (!questSaveDictionary.ContainsKey(questID))
        {
            return;
        }

        QuestSaveData saveData = questSaveDictionary[questID];

        if (saveData.state != QuestState.NotStarted)
        {
            return;
        }

        saveData.state = QuestState.InProgress;
    }

    // クエストの状態取得関数
    public QuestState GetQuestState(string questID)
    {
        if (questSaveDictionary.ContainsKey(questID))
        {
            return questSaveDictionary[questID].state;
        }
        else
        {
            return QuestState.NotStarted;
        }
    }

    // クエスト進行状況取得登録関数
    public void UpdateQuestProgress(string questID, int amount)
    {
        if(questSaveDictionary.ContainsKey(questID))
        {
            QuestSaveData saveData = questSaveDictionary[questID];
            saveData.currentProgress = saveData.currentProgress + amount;
        }
    }
}
