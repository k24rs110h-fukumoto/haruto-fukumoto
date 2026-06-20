using UnityEngine;


public enum BGMType
{
    Title,
    FirstTown,
    FirstField,
    Battle,
    MiddleBoss,
    Boss

}

public enum SEType
{
    Button,
    Select,
    Attack,
    Damage,
    Heal,
    Item
}
[System.Serializable]
public class BGMData
{
    public BGMType type;
    public AudioClip clip;
}

[System.Serializable]
public class SEData
{
    public SEType type;
    public AudioClip clip;
}