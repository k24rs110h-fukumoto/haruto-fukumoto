using System.Collections.Generic;
using UnityEngine;

public class OpenedChestManager : MonoBehaviour
{
    public static OpenedChestManager Instance;

    private HashSet<string> openedChests;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        openedChests = new HashSet<string>();
    }

    public void AddOpenedChest(string chestID)
    {
        openedChests.Add(chestID);
    }

    public bool IsOpened(string chestID)
    {
        return openedChests.Contains(chestID);
    }
}