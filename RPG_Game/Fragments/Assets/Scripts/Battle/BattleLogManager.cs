using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleLogManager : MonoBehaviour
{
    public static BattleLogManager Instance;
    [SerializeField] private TextMeshProUGUI logText;
    private int maxLogCount = 10;
    private Queue<string> logs = new Queue<string>();
    
    void Awake()
    {
        Instance = this;
    }

    public void AddLog(string message)
    {
        logs.Enqueue(message);
        if(logs.Count > maxLogCount)
        {
            logs.Dequeue();
        }

        logText.text = string.Join("\n", logs);
    }
}