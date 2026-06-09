using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public PlayerData playerData;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (playerData == null)
        {
            playerData = new PlayerData();
        }

    }

    public void CreateNewPlayerData()
    {
        playerData = new PlayerData();

        playerData.playerName = "主人公";

        playerData.level = 1;
        playerData.currentExp = 0;
        playerData.nextExp = 30;
        playerData.currentGold = 0;

        playerData.baseHp = 100;
        playerData.baseAttack = 10;
        playerData.baseDefense = 5;
        playerData.baseSpeed = 5;

        playerData.maxHp = StatusCalculator.GetMaxHp(playerData.baseHp, playerData.level);
        playerData.currentHp = playerData.maxHp;

        playerData.attack = StatusCalculator.GetAttack(playerData.baseAttack, playerData.level);
        playerData.defense = StatusCalculator.GetDefense(playerData.baseDefense, playerData.level);
        playerData.speed = StatusCalculator.GetSpeed(playerData.baseSpeed, playerData.level);

        playerData.iconNumber = 0;

        playerData.currentSceneName = "VillageScene";
        playerData.currentSpawnPointID = "StartPoint";
    }

    public void AddExp(int exp)
    {
        playerData.currentExp = playerData.currentExp + exp;

        while (playerData.currentExp >= playerData.nextExp)
        {
            playerData.currentExp = playerData.currentExp - playerData.nextExp;
            LevelUp();
            playerData.nextExp = Mathf.RoundToInt(30 * Mathf.Pow(playerData.level, 1.5f));
        }
    }

    private void LevelUp()
    {
        playerData.level = playerData.level + 1;
        playerData.maxHp = StatusCalculator.GetMaxHp(playerData.baseHp, playerData.level);
        playerData.attack = StatusCalculator.GetAttack(playerData.baseAttack, playerData.level);
        playerData.defense = StatusCalculator.GetDefense(playerData.baseDefense, playerData.level);
        playerData.speed = StatusCalculator.GetSpeed(playerData.baseSpeed, playerData.level);
    }

    public void AddGold(int gold)
    {
        playerData.currentGold = playerData.currentGold + gold;
    }
}