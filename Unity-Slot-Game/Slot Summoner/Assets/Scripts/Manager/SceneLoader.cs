using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void LoadSlot()
    {
        SceneManager.LoadScene("SlotScene");
    }

    public void LoadBattle()
    {
        SceneManager.LoadScene("BattleScene");
    }

    public void LoadResult()
    {
        SceneManager.LoadScene("ResultScene");
    }
}