using UnityEngine;

[CreateAssetMenu(fileName = "SoundDatabase", menuName = "Game/Sound Database")]
public class SoundDatabase : ScriptableObject
{
    public BGMData[] bgmList;
    public SEData[] seList;
}