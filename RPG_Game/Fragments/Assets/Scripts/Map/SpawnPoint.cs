using System;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private string spawnID;

    public string GetSpawnID()
    {
        return spawnID;
    }
}
