using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void StartSlot()
    {
        GameData.Instance.ResetSlotData();
        SceneManager.LoadScene("SlotScene");
    }

    public void StartCustomBattle()
    {
        SceneManager.LoadScene("CustomBattleScene");
    }

    public void OpenGallery()
    {
        SceneManager.LoadScene("GalleryScene");
    }
}