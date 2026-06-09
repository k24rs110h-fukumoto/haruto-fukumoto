using UnityEngine;

[CreateAssetMenu(fileName = "CharacterImageDatabase", menuName = "Game/Character Image Database")]
public class CharacterImageDatabase : ScriptableObject
{
    public CharacterImageData[] characterImages;

    public CharacterImageData GetCharacterImageData(int iconNumber)
    {
        foreach (CharacterImageData data in characterImages)
        {
            if (data.iconNumber == iconNumber)
            {
                return data;
            }
        }

        Debug.LogWarning("CharacterImageData が見つかりません: " + iconNumber);
        return null;
    }

    public Sprite GetFaceImage(int iconNumber)
    {
        CharacterImageData data = GetCharacterImageData(iconNumber);

        if (data == null)
        {
            return null;
        }

        return data.standingImage;
    }
}