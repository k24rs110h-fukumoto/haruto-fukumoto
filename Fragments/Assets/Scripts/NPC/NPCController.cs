using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] private NPCData npcData;

    private bool canTalk;
    private bool isTalking;

    private void Update()
    {
        if (InputDelayManager.IsLocked())
        {
            return;
        }

        if (!canTalk)
        {
            return;
        }

        if (isTalking)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
        {
            Talk();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canTalk = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canTalk = false;
        }
    }

    private void Talk()
    {
        if (npcData == null)
        {
            Debug.LogWarning("NPCData is not set");
            return;
        }

        if (npcData.dialogueData == null)
        {
            Debug.LogWarning(npcData.npcName + " has no DialogueData");
            return;
        }

        isTalking = true;

        if (npcData.questData != null)
        {
            TalkWithQuest();
            return;
        }

        ShowNpcDialogue(null);
    }

    private void TalkWithQuest()
    {
        QuestState state = QuestManager.Instance.GetQuestState(npcData.questData.questID);

        switch (state)
        {
            case QuestState.NotStarted:
                ShowNpcDialogue(() =>
                {
                    QuestManager.Instance.AcceptQuest(npcData.questData.questID);
                    isTalking = false;
                });
                break;

            case QuestState.InProgress:
                ShowNpcDialogue(() =>
                {
                    isTalking = false;
                });
                break;

            case QuestState.Completed:
                ShowNpcDialogue(() =>
                {
                    isTalking = false;
                });
                break;

            case QuestState.RewardReceived:
                ShowNpcDialogue(() =>
                {
                    isTalking = false;
                });
                break;
        }
    }

    private void ShowNpcDialogue(System.Action onFinished)
    {
        DialogueManager.Instance.ShowDialogue(
            npcData.npcName,
            npcData.npcImage,
            npcData.dialogueData,
            () =>
            {
                if (onFinished != null)
                {
                    onFinished.Invoke();
                }

                isTalking = false;
                InputDelayManager.Lock(0.2f);
            }
        );
    }
}