using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveInput;
    private Rigidbody2D rb;

    [SerializeField] private float moveSpeed = 5f;

    // This stores the initial scale so we don't lose the original size
    private Vector3 initialScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        initialScale = transform.localScale;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        HandleDirectionalFlip();
    }

    private void ApplyMovement()
    {
        Vector2 movement = new Vector2(moveInput.x, moveInput.y) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    private void HandleDirectionalFlip()
    {
        // If moveInput.x is negative, the player is moving left
        if (moveInput.x < -0.01f)
        {
            // Flip to face left (invert the X scale)
            transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
        }
        // If moveInput.x is positive, the player is moving right
        else if (moveInput.x > 0.01f)
        {
            // Set back to original scale to face right
            transform.localScale = initialScale;
        }
    }
}