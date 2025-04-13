using UnityEngine;

public class WindAbility : MonoBehaviour, IAbility
{
    private Rigidbody2D body;
    private SpriteRenderer sprite;

    private bool isHoldingJump;
    private bool canDoubleJump = true;

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

        if (canDash && Input.GetMouseButtonDown(0))
        {
            float direction = sprite.flipX ? -1f : 1f;
            body.linearVelocity = new Vector2(direction * dashForce, 0f);
            canDash = false;
            Invoke(nameof(StopDash), dashDuration);
            Invoke(nameof(EnableDash), dashCooldown);
        }

        // Второй прыжок (на правую кнопку мыши)
        if (canDoubleJump && Input.GetMouseButtonDown(1))
        {
            Debug.Log("Double Jump!");
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
            canDoubleJump = false;
        }
    }

    public void OnFixedUpdate()
    {
        if (isHoldingJump && body.linearVelocity.y < 0)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, body.linearVelocity.y * fallSlowFactor);
        }
    }

    public void OnJump()
    {
        Debug.Log("Wind Jump!");
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        canDoubleJump = true; // Разрешаем второй прыжок после первого
    }

    private void StopDash() => body.linearVelocity = new Vector2(0f, body.linearVelocity.y);
    private void EnableDash() => canDash = true;
}
