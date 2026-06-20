using UnityEngine;

public class TeleportArea : MonoBehaviour
{
    [SerializeField] private Transform targetPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        CharacterController controller = other.GetComponent<CharacterController>();

        if (controller != null)
        {
            controller.enabled = false;
            other.transform.SetPositionAndRotation(
                targetPoint.position,
                targetPoint.rotation);
            controller.enabled = true;
        }
        else
        {
            other.transform.SetPositionAndRotation(
                targetPoint.position,
                targetPoint.rotation);
        }
    }
}