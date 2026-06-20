using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Setting")]
    [SerializeField] private float interactDistance = 2.5f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private PromptUIManager promptUIManager;

    private bool isHit;
    private RaycastHit currentHit;
    private IInteractable currentInteractable;

    private void Update()
    {
        CheckInteractable();
    }

    private void CheckInteractable()
    {
        if (cameraTransform == null)
        {
            return;
        }

        isHit = Physics.Raycast(
            cameraTransform.position,
            cameraTransform.forward,
            out currentHit,
            interactDistance,
            interactLayer
        );

        if (!isHit)
        {
            currentInteractable = null;

            if (promptUIManager != null)
            {
                promptUIManager.Hide();
            }

            return;
        }

        currentInteractable = currentHit.collider.GetComponent<IInteractable>();

        if (currentInteractable == null)
        {
            if (promptUIManager != null)
            {
                promptUIManager.Hide();
            }

            return;
        }

        if (promptUIManager != null)
        {
            promptUIManager.Show(currentInteractable.GetPrompt());
        }
    }

    public void TryInteract()
    {
        if (currentInteractable == null)
        {
            return;
        }

        currentInteractable.Interact();
    }

    private void OnDrawGizmosSelected()
    {
        if (cameraTransform == null) return;

        Gizmos.DrawRay(cameraTransform.position, cameraTransform.forward * interactDistance);
    }
}