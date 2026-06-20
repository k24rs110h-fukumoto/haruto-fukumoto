using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Game/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public Sprite characterImage;

    public int baseHp;
    public int baseAttack;
}