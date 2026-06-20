using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Vector2 moveInput;
    private Rigidbody2D rb;
    private PlayerVisualController playerVisualController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerVisualController = GetComponent<PlayerVisualController>();
    }

    private void Update()
    {
        if (!GameStateManager.IsState(GameStateManager.GameState.Field))
        {
            moveInput = Vector2.zero;
            return;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(x, y).normalized;

        if (playerVisualController != null)
        {
            playerVisualController.UpdateDirection(moveInput);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}