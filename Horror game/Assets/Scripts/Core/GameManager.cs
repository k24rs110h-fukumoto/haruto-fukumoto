using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        GameStateManager.SetState(GameStateManager.GameState.Title);
    }

    
}