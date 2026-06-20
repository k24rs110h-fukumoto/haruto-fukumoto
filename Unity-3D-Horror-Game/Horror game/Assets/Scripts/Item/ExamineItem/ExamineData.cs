using UnityEngine;

[CreateAssetMenu(fileName = "ExamineData", menuName = "Game/ExamineData")]
public class ExamineData : ScriptableObject
{
    public string title;
    public string message;
    public Sprite image;
    public ExamineType examineType;
}