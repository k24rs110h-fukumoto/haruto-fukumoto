using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameStateManager.SetState(GameStateManager.GameState.Ending);
            SceneManager.LoadScene("Ending");
        }
    }
}