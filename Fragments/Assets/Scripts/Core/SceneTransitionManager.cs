using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    private static string nextSceneName;
    private static string nextSpawnID;
    public static bool isDirectSpawn;
    public static Vector3 directSpawnPosition;

    private const string LoadingSceneName = "LoadingScene";

    public static void LoadScene(string sceneName, string spawnID)
    {
        nextSceneName = sceneName;
        nextSpawnID = spawnID;

        SceneManager.LoadScene(LoadingSceneName);
    }

    public static string GetNextSceneName()
    {
        return nextSceneName;
    }

    public static string GetNextSpawnID()
    {
        return nextSpawnID;
    }

    public static void LoadSceneAtPosition(string sceneName, Vector3 position)
    {
        nextSceneName = sceneName;
        isDirectSpawn = true;
        directSpawnPosition = position;

        SceneManager.LoadScene(LoadingSceneName);
    }
}