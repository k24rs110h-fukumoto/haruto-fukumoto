using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameState currentState;

    private float playTime;

    private void Update()
    {
        if(currentState == GameState.Playing)
        {
            playTime += Time.deltaTime;
        }
    }

    public void StartGame()
    {
        currentState = GameState.Playing;
    }

    public void PauseGame()
    {
        currentState = GameState.Pause;
    }

    public void ResumeGame()
    {
        currentState = GameState.Playing;
    }

    public void EndGame()
    {
        currentState = GameState.Result;
    }
}