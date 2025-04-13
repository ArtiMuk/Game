using UnityEngine;

public class WindAbility : MonoBehaviour, IAbility
{
    private Rigidbody2D body;
    private SpriteRenderer sprite;

    private bool isHoldingJump;
    private bool hasDoubleJumped = false;

    [SerializeField] private float fallSlowFactor = 0.2f;
    [SerializeField] private float jumpForce = 13f;
    [SerializeField] private float dashForce = 30f;
    [SerializeField] private float dashCooldown = 0.8f;
    [SerializeField] private float dashDuration = 0.1f;

    private bool canDash = true;

    public void Init(Rigidbody2D rb, SpriteRenderer sr)
    {
        body = rb;
        sprite = sr;
    }

    public void OnUpdate()
    {
        isHoldingJump = Input.GetButton("Jump");

        if (canDash && Input.GetKeyDown(KeyCode.E))
        {
            float direction = sprite.flipX ? -1f : 1f;
            body.linearVelocity = new Vector2(direction * dashForce, 0f);
            canDash = false;
            Invoke(nameof(StopDash), dashDuration);
            Invoke(nameof(EnableDash), dashCooldown);
        }

        // Двойной прыжок только в воздухе
        if (!hasDoubleJumped && !IsGrounded() && Input.GetButtonDown("Jump"))
        {
            Debug.Log("Double Jump!");
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
            hasDoubleJumped = true;
        }
    }

    public void OnFixedUpdate()
    {
        // Планирование только при удержании RightShift
        if (Input.GetKey(KeyCode.RightShift) && body.linearVelocity.y < 0)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, body.linearVelocity.y * fallSlowFactor);
        }
    }

    public void OnJump()
    {
        Debug.Log("Wind Jump!");
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        hasDoubleJumped = false; // Сбрасываем флаг на 2й прыжок после первого
    }

    public void OnLand()
    {
        hasDoubleJumped = false; // Сброс при касании земли
    }

    private void StopDash() => body.linearVelocity = new Vector2(0f, body.linearVelocity.y);
    private void EnableDash() => canDash = true;

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircleAll(transform.position, 0.3f).Length > 1;
    }
}
