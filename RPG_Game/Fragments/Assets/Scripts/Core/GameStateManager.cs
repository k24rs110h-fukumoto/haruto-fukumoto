using UnityEngine;

public class GameStateManager: MonoBehaviour
{
    // ゲームの状態を管理
    public enum GameState
    {
        Title, // タイトル画面
        Field, // フィールド
        Battle, // バトル
        Menu, // メニュー
        GameOver
    }

    public static GameState currentState;

    public static void SetState(GameState newState)
    {
        // 状態を変更する処理
        currentState = newState;

        Debug.Log("Game State changed to: " + currentState);
    }

    public static GameState GetState()
    {
        return currentState;
    }

    public static bool IsState(GameState state)
    {
        return currentState == state;
    }
}
