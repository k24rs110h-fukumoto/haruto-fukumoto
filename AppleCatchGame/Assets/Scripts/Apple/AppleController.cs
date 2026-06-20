using UnityEngine;

public class AppleController : MonoBehaviour
{
    [Header("Apple Setting")]
    [SerializeField] private float fallSpeed = 3f;
    [SerializeField] private float destroyY = -6f;
    [SerializeField] private AudioClip getSE;

    private AppleCatchGameManager gameManager;

    private void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        if (transform.position.y <= destroyY)
        {
            Destroy(gameObject);
        }
    }

    public void SetGameManager(AppleCatchGameManager manager)
    {
        gameManager = manager;
    }

    private void OnMouseDown()
    {
        if (gameManager == null)
        {
            return;
        }

        if (!gameManager.IsPlaying())
        {
            return;
        }

        gameManager.AddScore();
        AudioManager.Instance.PlaySE(getSE);
        Destroy(gameObject);
    }
}