using UnityEngine;
using TMPro;

public class ExpectationManager : MonoBehaviour
{
    public TextMeshProUGUI expectationText;

    void Update()
    {
        if (GameData.Instance == null) return;

        int value = GameData.Instance.expectation;

        expectationText.text = "期待度 " + value + "%";

        if (value >= 180)
        {
            expectationText.color = Color.magenta;
        }
        else if (value >= 120)
        {
            expectationText.color = Color.yellow;
        }
        else if (value >= 70)
        {
            expectationText.color = Color.red;
        }
        else if (value >= 40)
        {
            expectationText.color = Color.cyan;
        }
        else
        {
            expectationText.color = Color.white;
        }
    }
}