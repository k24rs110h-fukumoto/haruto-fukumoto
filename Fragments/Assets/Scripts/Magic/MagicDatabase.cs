using UnityEngine;

public class MagicDatabase : MonoBehaviour
{
    public static MagicDatabase Instance;
    public MagicData[] magicDataList;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        magicDataList = Resources.LoadAll<MagicData>("Magics");
    }

    public MagicData GetMagicData(string magicID)
    {
        foreach(MagicData magicData in magicDataList)
        {
            if(magicData.magicID == magicID)
            {
                return magicData;
            }
        }
        return null;
    }
}