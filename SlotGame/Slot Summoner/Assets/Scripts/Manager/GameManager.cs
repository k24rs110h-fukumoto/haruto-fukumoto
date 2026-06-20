using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int expectation = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void AddExpectation(int value)
    {
        expectation += value;
    }
}