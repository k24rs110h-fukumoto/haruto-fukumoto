using UnityEngine;

public class PlayerFootstep : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private float walkStepInterval = 0.6f;
    [SerializeField] private float runStepInterval = 0.35f;
    [SerializeField] private float crouchStepInterval = 0.9f;

    private float stepTimer;

    public void UpdateFootstep(Vector2 moveInput, bool isRun, bool isCrouch)
    {
        if (controller == null)
        {
            return;
        }

        if (!controller.isGrounded)
        {
            stepTimer = 0f;
            return;
        }

        if (moveInput.magnitude <= 0.1f)
        {
            stepTimer = 0f;
            return;
        }

        float interval = walkStepInterval;

        if (isCrouch)
        {
            interval = crouchStepInterval;
        }
        else if (isRun)
        {
            interval = runStepInterval;
        }

        stepTimer += Time.deltaTime;

        if (stepTimer >= interval)
        {
            PlayFootstep();
            stepTimer = 0f;
        }
    }

    private void PlayFootstep()
    {
        if (footstepClips == null || footstepClips.Length == 0)
        {
            return;
        }

        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySE(clip);
        }
    }
}