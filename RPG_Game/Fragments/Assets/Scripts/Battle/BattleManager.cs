using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    public EnemyData[] enemyDataList;
    public List<BattleEnemyData> battleEnemyDataList = new List<BattleEnemyData>();
    public PlayerBattleData playerBattleData;
    private int totalExp;
    private int totalGold;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        enemyDataList = BattleDataManager.Instance.currentEnemyDataList;
        foreach (EnemyData enemy in enemyDataList)
        {
            BattleEnemyData battleEnemyData = new BattleEnemyData();
            battleEnemyData.enemyData = enemy;
            battleEnemyData.level = EnemyLevelManager.Instance.GetEnemyLevel(enemy, PlayerManager.Instance.playerData.level, GetAreaDifficulty(SceneManager.GetActiveScene()));

            battleEnemyData.maxHp = StatusCalculator.GetMaxHp(enemy.maxHp, battleEnemyData.level);
            battleEnemyData.currentHp = battleEnemyData.maxHp;
            battleEnemyData.attack = StatusCalculator.GetAttack(enemy.attack, battleEnemyData.level);
            battleEnemyData.defense = StatusCalculator.GetDefense(enemy.defense, battleEnemyData.level);
            battleEnemyData.speed = StatusCalculator.GetSpeed(enemy.speed, battleEnemyData.level);
            battleEnemyDataList.Add(battleEnemyData);
        }
        CreatePlayerBattleData();
        BattleUIManager.Instance.RefreshUI();
        totalExp = 0;
        totalGold = 0;
    }

    // プレイヤーバトルデータ作成
    private void CreatePlayerBattleData()
    {
        playerBattleData = new PlayerBattleData();
        playerBattleData.maxHp = PlayerManager.Instance.playerData.maxHp;
        playerBattleData.currentHp = PlayerManager.Instance.playerData.currentHp;
        playerBattleData.attack = PlayerManager.Instance.playerData.attack;
        playerBattleData.defense = PlayerManager.Instance.playerData.defense;
        playerBattleData.speed = PlayerManager.Instance.playerData.speed;
    }

    public void MagicAttackAllEnemy(MagicData magic)
    {
        BattleLogManager.Instance.AddLog(magic.magicName + " を使用！");
        MagicManager.Instance.UseMagic(magic.magicID, 1);

        for (int i = battleEnemyDataList.Count - 1; i >= 0; i--)
        {
            bool critical;
            BattleEnemyData battleEnemyData = battleEnemyDataList[i];

            int damage = CalculateDamage(magic.power, battleEnemyData.defense, out critical);
            damage = damage + playerBattleData.attack;
            float multiplier = ElementManager.Instance.GetElementMultiplier(magic.element, battleEnemyData.enemyData.element);
            damage = Mathf.RoundToInt(damage * multiplier);

            if (multiplier > 1f)
            {
                BattleLogManager.Instance.AddLog("弱点を突いた！");
            }
            else if (multiplier < 1f)
            {
                BattleLogManager.Instance.AddLog("効果はいまひとつ...");
            }
            if (critical)
            {
                BattleLogManager.Instance.AddLog("クリティカル攻撃！");
            }
            DamageEnemy(battleEnemyData, damage);
        }
    }

    private void ExecuteTurn(System.Action playerAction)
    {
        BattleEnemyData[] enemies = new BattleEnemyData[battleEnemyDataList.Count];

        for (int i = 0; i < battleEnemyDataList.Count; i++)
        {
            enemies[i] = battleEnemyDataList[i];
        }

        System.Array.Sort(enemies, (a, b) => b.speed.CompareTo(a.speed));

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].speed > playerBattleData.speed)
            {
                EnemyAttack(enemies[i]);

                if (playerBattleData.currentHp <= 0)
                {
                    return;
                }
            }
        }

        playerAction();

        if (battleEnemyDataList.Count == 0)
        {
            return;
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].speed <= playerBattleData.speed)
            {
                if (!battleEnemyDataList.Contains(enemies[i]))
                {
                    continue;
                }

                EnemyAttack(enemies[i]);

                if (playerBattleData.currentHp <= 0)
                {
                    return;
                }
            }
        }

        BattleMenuManager.Instance.OpenBattleMenu();
    }

    public void DecideOrder(BattleEnemyData enemy, InventoryItem item)
    {
        ExecuteTurn(() =>
        {
            switch (item.itemData.itemType)
            {
                case ItemType.Recovery:
                    playerBattleData.currentHp += item.itemData.heal;

                    if (playerBattleData.currentHp > playerBattleData.maxHp)
                    {
                        playerBattleData.currentHp = playerBattleData.maxHp;
                    }

                    BattleLogManager.Instance.AddLog(item.itemData.itemName + " を使った！");
                    BattleLogManager.Instance.AddLog(item.itemData.heal + " 回復した！");
                    break;

                case ItemType.BattleItem:
                    AttackEnemy(enemy, item.itemData.attack);
                    break;
            }

            InventoryManager.Instance.RemoveItem(item.itemData.itemID, 1);
            BattleUIManager.Instance.RefreshUI();
        });
    }

    public void DecideOrder(BattleEnemyData targetEnemyData, int attack)
    {
        ExecuteTurn(() =>
        {
            AttackEnemy(targetEnemyData, attack);
        });
    }

    public void DecideOrder(BattleEnemyData targetEnemyData, MagicData magic, bool isTargetAll)
    {
        ExecuteTurn(() =>
        {
            switch (magic.magicType)
            {
                case MagicType.Attack:
                    if (isTargetAll)
                    {
                        MagicAttackAllEnemy(magic);
                    }
                    else
                    {
                        MagicAttackEnemy(targetEnemyData, magic);
                    }
                    break;

                case MagicType.Heal:
                    HealPlayer(magic);
                    break;
            }
        });
    }

    private void MagicAttackEnemy(BattleEnemyData battleEnemyData, MagicData magic)
    {
        bool critical;

        BattleLogManager.Instance.AddLog(magic.magicName + " を使用！");
        MagicManager.Instance.UseMagic(magic.magicID, 1);

        int attackPower = magic.power + playerBattleData.attack;
        int damage = CalculateDamage(attackPower, battleEnemyData.defense, out critical);

        float multiplier = ElementManager.Instance.GetElementMultiplier(
            magic.element,
            battleEnemyData.enemyData.element
        );

        damage = Mathf.RoundToInt(damage * multiplier);

        if (multiplier > 1f)
        {
            BattleLogManager.Instance.AddLog("弱点を突いた！");
        }
        else if (multiplier < 1f)
        {
            BattleLogManager.Instance.AddLog("効果はいまひとつ...");
        }

        if (critical)
        {
            BattleLogManager.Instance.AddLog("クリティカル攻撃！");
        }

        DamageEnemy(battleEnemyData, damage);
    }

    private void AttackEnemy(BattleEnemyData battleEnemyData, int attack)
    {
        bool critical;
        int damage = CalculateDamage(attack, battleEnemyData.defense, out critical);

        if (critical)
        {
            BattleLogManager.Instance.AddLog("クリティカル攻撃！");
        }
        DamageEnemy(battleEnemyData, damage);
    }

    // 敵へのダメージ反映
    private void DamageEnemy(BattleEnemyData enemy, int damage)
    {
        if (!IsHit())
        {
            BattleLogManager.Instance.AddLog(enemy.enemyData.enemyName + "には当たらなかった");
            return;
        }
        ApplyDamage(enemy, damage);
        CheckEnemyDeath(enemy);
        CheckBattleWin();
    }

    private void ApplyDamage(BattleEnemyData enemy, int damage)
    {
        enemy.currentHp = enemy.currentHp - damage;
        if (enemy.currentHp < 0)
        {
            enemy.currentHp = 0;
        }
        BattleLogManager.Instance.AddLog(enemy.enemyData.enemyName + "に" + damage + " ダメージ!");
        BattleUIManager.Instance.RefreshUI();
    }

    private void CheckEnemyDeath(BattleEnemyData enemy)
    {
        if (enemy.currentHp > 0)
        {
            return;
        }

        ProcessEnemyDeath(enemy);
    }

    private void ProcessEnemyDeath(BattleEnemyData enemy)
    {
        totalExp = totalExp + enemy.GetExpReward();
        totalGold = totalGold + enemy.GetGoldReward();
        TryDropItem(enemy);

        battleEnemyDataList.Remove(enemy);
        BattleUIManager.Instance.RefreshUI();
    }

    private void CheckBattleWin()
    {
        if (battleEnemyDataList.Count == 0)
        {
            WinBattle();
        }
    }

    private void EnemyAttack(BattleEnemyData enemyData)
    {
        bool critical;
        if (!battleEnemyDataList.Contains(enemyData))
        {
            return;
        }
        BattleLogManager.Instance.AddLog(enemyData.enemyData.enemyName + "の攻撃！");
        if (enemyData.statusEffect == BattleEnemyData.StatusEffect.Sleep)
        {
            BattleLogManager.Instance.AddLog(enemyData.enemyData.enemyName + "は眠っている");
            return;
        }
        if (enemyData.statusEffect == BattleEnemyData.StatusEffect.Paralyze)
        {
            if (IsActive())
            {
                BattleLogManager.Instance.AddLog(enemyData.enemyData.enemyName + "は麻痺して動けない");
                return;
            }
        }
        if (!IsHit())
        {
            BattleLogManager.Instance.AddLog("ミス");
            return;
        }

        int damage = CalculateDamage(enemyData.attack, playerBattleData.defense, out critical);

        playerBattleData.currentHp = playerBattleData.currentHp - damage;

        BattleUIManager.Instance.RefreshUI();

        if (critical)
        {
            BattleLogManager.Instance.AddLog("クリティカル攻撃！");
        }
        BattleLogManager.Instance.AddLog(damage + " ダメージ受けた！");

        if (playerBattleData.currentHp <= 0)
        {
            LoseBattle();
        }
    }

    public void HealPlayer(MagicData magic)
    {
        BattleLogManager.Instance.AddLog(magic.magicName + " を使用！");

        MagicManager.Instance.UseMagic(magic.magicID, 1);

        int healAmount = magic.power;

        RecoverPlayer(healAmount);

        BattleUIManager.Instance.RefreshUI();

        BattleLogManager.Instance.AddLog(healAmount + " 回復した！");
    }

    private void RecoverPlayer(int healAmount)
    {
        playerBattleData.currentHp = playerBattleData.currentHp + healAmount;

        if (playerBattleData.currentHp > playerBattleData.maxHp)
        {
            playerBattleData.currentHp = playerBattleData.maxHp;
        }
    }

    private void WinBattle()
    {
        if (!string.IsNullOrEmpty(BattleDataManager.Instance.currentFieldEnemyID))
        {
            DefeatedEnemyManager.Instance.AddDefeatedEnemy(
                BattleDataManager.Instance.currentFieldEnemyID
            );
        }

        PlayerManager.Instance.AddExp(totalExp);
        PlayerManager.Instance.AddGold(totalGold);

        BattleLogManager.Instance.AddLog(totalExp + " EXP 獲得");
        BattleLogManager.Instance.AddLog(totalGold + "G 獲得");
        PlayerManager.Instance.playerData.currentHp = playerBattleData.currentHp;

        SceneTransitionManager.LoadSceneAtPosition(
            BattleDataManager.Instance.returnSceneName,
            BattleDataManager.Instance.returnPlayerPosition
        );

        GameStateManager.SetState(GameStateManager.GameState.Field);
        Destroy(gameObject);
    }

    private void LoseBattle()
    {
        PlayerManager.Instance.playerData.currentHp = playerBattleData.maxHp;

        SceneTransitionManager.LoadScene("Area01_Town", "Area01_Town_Start");

        GameStateManager.SetState(GameStateManager.GameState.Field);
        Destroy(gameObject);
    }

    // デバフ付与
    private void AddStatusEffect(BattleEnemyData enemy, BattleEnemyData.StatusEffect effect)
    {
        enemy.statusEffect = effect;
        BattleLogManager.Instance.AddLog(enemy.enemyData.enemyName + "は" + GetStatusName(effect) + "になった！");
    }

    // デバフ名取得
    private string GetStatusName(BattleEnemyData.StatusEffect effect)
    {
        switch (effect)
        {
            case BattleEnemyData.StatusEffect.Poison:
                return "毒";
            case BattleEnemyData.StatusEffect.Sleep:
                return "眠り";
            case BattleEnemyData.StatusEffect.Paralyze:
                return "麻痺";
            default:
                return "";
        }
    }

    // 麻痺判定
    private bool IsActive()
    {
        int value = Random.Range(0, 100);
        if (value < 50)
        {
            return true;
        }

        return false;
    }

    // クリティカル判定
    private bool IsCritical()
    {
        int value = Random.Range(0, 100);
        if (value < 20)
        {
            return true;
        }

        return false;
    }

    // 命中判定
    private bool IsHit()
    {
        int value = Random.Range(0, 100);
        if (value < 90)
        {
            return true;
        }

        return false;
    }

    // ダメージ計算
    private int CalculateDamage(int attack, int defense, out bool critical)
    {
        critical = IsCritical();

        int damage = attack - defense;

        if (damage < 1)
        {
            damage = 1;
        }
        if (critical)
        {
            damage = Mathf.RoundToInt(damage * 1.5f);
        }

        return damage;
    }

    private void ProcessStatusEffect(BattleEnemyData enemy)
    {
        switch (enemy.statusEffect)
        {
            case BattleEnemyData.StatusEffect.None:
                return;
            case BattleEnemyData.StatusEffect.Poison:
                int damage = Mathf.RoundToInt(enemy.maxHp * 0.1f);
                BattleLogManager.Instance.AddLog("猛毒が" + enemy.enemyData.enemyName + "襲う");
                DamageEnemy(enemy, damage);
                return;
            case BattleEnemyData.StatusEffect.Sleep:
                return;
            case BattleEnemyData.StatusEffect.Paralyze:
                return;
        }
    }


    private void TryDropItem(BattleEnemyData enemy)
    {
        if (enemy.enemyData.dropItem == null)
        {
            return;
        }

        float randomValue = Random.value;
        if (randomValue < enemy.enemyData.dropRate)
        {
            InventoryManager.Instance.AddItem(enemy.enemyData.dropItem, 1);
            BattleLogManager.Instance.AddLog(enemy.enemyData.enemyName + " が " + enemy.enemyData.dropItem.itemName + " を落とした");
        }
    }

    public void TryEscape()
    {
        BattleLogManager.Instance.AddLog("逃げようとした！");

        int random = Random.Range(0, 100);

        if (random < 80)
        {
            EscapeSuccess();
        }
        else
        {
            BattleLogManager.Instance.AddLog("逃げられなかった！");
            EscapeFailed();
            BattleMenuManager.Instance.OpenBattleMenu();
        }
    }

    private void EscapeSuccess()
    {
        PlayerManager.Instance.playerData.currentHp = playerBattleData.currentHp;
        BattleDataManager.Instance.shouldDisableFieldEnemyOnce = true;
        SceneTransitionManager.LoadSceneAtPosition(
            BattleDataManager.Instance.returnSceneName,
            BattleDataManager.Instance.returnPlayerPosition
        );
        GameStateManager.SetState(GameStateManager.GameState.Field);

        Destroy(gameObject);
    }

    private void EscapeFailed()
    {
        BattleEnemyData[] battleEnemyDatas = new BattleEnemyData[battleEnemyDataList.Count]; ;

        for (int i = 0; i < battleEnemyDataList.Count; i++)
        {
            battleEnemyDatas[i] = battleEnemyDataList[i];
        }
        System.Array.Sort(battleEnemyDatas, (a, b) => b.speed.CompareTo(a.speed));
        for (int i = 0; i < battleEnemyDataList.Count; i++)
        {
            EnemyAttack(battleEnemyDatas[i]);
        }
    }

    private int GetAreaDifficulty(Scene scene)
    {
        switch (scene.name)
        {
            case "Area01_Field":
                return 1;
            case "Area01_Forest":
                return 2;
            default:
                return 1;
        }
    }
}