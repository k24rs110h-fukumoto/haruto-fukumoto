using UnityEngine;

public class RequiredItemDoorObject : RequiredItemObject
{
    [SerializeField] private GameObject lockedDoorControllObject;
    [SerializeField] private GameObject lockedDoorObject;
    [SerializeField] private GameObject openedDoorObject;
    [SerializeField] private AudioClip openSE;

    private bool isOpened;

    public override string GetPrompt()
    {
        if (isOpened)
        {
            return "";
        }

        return base.GetPrompt();
    }

    protected override void Success()
    {
        if (isOpened)
        {
            return;
        }

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySE(openSE);
        }

        base.Success();

        isOpened = true;

        if (lockedDoorObject != null)
        {
            lockedDoorObject.SetActive(false);
            lockedDoorControllObject.SetActive(false);
        }

        if (openedDoorObject != null)
        {
            openedDoorObject.SetActive(true);
        }
    }
}