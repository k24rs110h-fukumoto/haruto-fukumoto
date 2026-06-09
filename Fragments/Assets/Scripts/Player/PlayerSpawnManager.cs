using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private string defaultSpawnID;
    private string spawnID;
    private Dictionary<string, SpawnPoint> spawnPointDictionary = new Dictionary<string, SpawnPoint>();
    public void Awake()
    {
        GameStateManager.SetState(GameStateManager.GameState.Field);
        RegisterSpawnPoints();
        spawnID = SceneTransitionManager.GetNextSpawnID();
        if (string.IsNullOrEmpty(spawnID))
        {
            spawnID = defaultSpawnID;
        }
        if (SceneTransitionManager.isDirectSpawn)
        {
            DirectSpawnPlayer(SceneTransitionManager.directSpawnPosition);
            SceneTransitionManager.isDirectSpawn = false;
        }
        else
        {
            SpawnPlayer();
        }
    }

    private void RegisterSpawnPoints()
    {
        SpawnPoint[] spawnPoints = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);

        foreach (SpawnPoint point in spawnPoints)
        {
            string id = point.GetSpawnID();
            if (spawnPointDictionary.ContainsKey(id))
            {
                Debug.LogWarning(id + " already exists");
            }
            else
            {
                spawnPointDictionary.Add(id, point);
            }
        }
    }

    private void SpawnPlayer()
    {
        if (spawnPointDictionary.ContainsKey(spawnID))
        {
            SpawnPoint spawnPoint = spawnPointDictionary[spawnID];
            Instantiate(playerPrefab, spawnPoint.transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning(spawnID + " is not found");
        }
    }

    private void DirectSpawnPlayer(Vector3 playerPos)
    {
        Instantiate(playerPrefab, playerPos, Quaternion.identity);
    }
}
