using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Game/QuestData")]
public class QuestData : ScriptableObject
{
    // クエストのID
    public string questID;
    // クエストの名前
    public string title;
    // クエストの説明
    [TextArea] public string description;
    // 獲得Gold
    public int rewardGold;
    // 獲得アイテム
    //pubilc Item rewardItem;
    // 獲得Xp
    public int rewardExp;
    // クエストの種類
    public QuestType questType;
    // クエストの対象
    public string targetID;
    // 必要数
    public int requiredProgress;
    
}
