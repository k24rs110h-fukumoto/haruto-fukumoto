using UnityEngine;


public class MagicPickup : MonoBehaviour
{
    [SerializeField] private MagicData magicData;
    [SerializeField] private int amount;
    private bool canPickup;

    public void Update()
    {
        if (canPickup == true)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
            {
                MagicManager.Instance.AddMagic(magicData, amount);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = false;
        }
    }
}