using UnityEngine;

public class GameEntry : MonoBehaviour, IChildGame
{
    public GameManager gameManager;

    public void StartGame()
    {
        gameManager.StartGame();
    }

    public void PauseGame()
    {
        gameManager.PauseGame();
    }

    public void ResumeGame()
    {
        gameManager.ResumeGame();
    }

    public void EndGame()
    {
        gameManager.EndGame();
    }
}