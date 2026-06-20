using UnityEngine;

public class ExamineObject : MonoBehaviour, IInteractable
{
    [SerializeField] private ExamineData examineData;
    private InteractionUIManager interactionUIManager;

    private void Awake()
    {
        interactionUIManager = FindFirstObjectByType<InteractionUIManager>();
        if (interactionUIManager == null)
        {
            return;
        }
    }

    public void Interact()
    {
        if (interactionUIManager == null)
        {
            return;
        }
        if (interactionUIManager.IsOpen)
        {
            interactionUIManager.Hide();
        }
        else
        {
            switch (examineData.examineType)
            {
                case ExamineType.Message:
                    interactionUIManager.ShowMessage(examineData.message);
                    return;
                case ExamineType.Image:
                    interactionUIManager.ShowImage(examineData.image);
                    return;
            }
        }
    }

    public string GetPrompt()
    {
        return "[E] 調べる";
    }
}