using UnityEngine;

public class StageGate : MonoBehaviour
{
    [SerializeField] private string targetSceneName;
    [SerializeField] private string targetSpawnID;
    private bool isTransitioning;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (isTransitioning)
        {
            return;
        }

        isTransitioning = true;
        SceneTransitionManager.LoadScene(targetSceneName, targetSpawnID);
    }
}
