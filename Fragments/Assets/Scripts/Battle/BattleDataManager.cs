using UnityEngine;


public class BattleDataManager : MonoBehaviour
{
    public static BattleDataManager Instance;
    public EnemyData[] currentEnemyDataList;
    public string returnSceneName;
    public Vector3 returnPlayerPosition;
    public string currentFieldEnemyID;
    public bool shouldDisableFieldEnemyOnce;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    

    public void SetReturnData()
    {
        
    }
}