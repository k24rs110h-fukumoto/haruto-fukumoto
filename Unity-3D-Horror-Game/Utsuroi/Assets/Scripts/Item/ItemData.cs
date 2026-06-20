using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Game/ItemData")]
public class ItemData : ScriptableObject
{
    public string ItemID;
    public string ItemName;
    public Sprite icon;
    public string Description;
    public GameObject dropPrefab;
}