using UnityEngine;

public class PlayerVisualController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CharacterImageDatabase characterImageDatabase;

    private CharacterImageData currentImageData;
    private Vector2 lastDirection = Vector2.down;

    private void Start()
    {
        ApplyCharacterImage();
    }

    public void ApplyCharacterImage()
    {
        if (PlayerManager.Instance == null)
        {
            Debug.LogError("PlayerManager がありません。");
            return;
        }

        int iconNumber = PlayerManager.Instance.playerData.iconNumber;

        currentImageData = characterImageDatabase.GetCharacterImageData(iconNumber);

        if (currentImageData == null)
        {
            return;
        }

        spriteRenderer.sprite = currentImageData.downSprite;
    }

    public void UpdateDirection(Vector2 moveInput)
    {
        if (currentImageData == null)
        {
            return;
        }

        if (moveInput == Vector2.zero)
        {
            return;
        }

        lastDirection = moveInput;

        if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
        {
            if (moveInput.x > 0)
            {
                spriteRenderer.sprite = currentImageData.rightSprite;
            }
            else
            {
                spriteRenderer.sprite = currentImageData.leftSprite;
            }
        }
        else
        {
            if (moveInput.y > 0)
            {
                spriteRenderer.sprite = currentImageData.upSprite;
            }
            else
            {
                spriteRenderer.sprite = currentImageData.downSprite;
            }
        }
    }
}