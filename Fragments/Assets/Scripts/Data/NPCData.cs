using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "Game/NPCData")]
public class NPCData : ScriptableObject
{
    public string npcName;
    public Sprite npcImage;
    public DialogueData dialogueData;
    public QuestData questData;
    public ShopData shopData;
}