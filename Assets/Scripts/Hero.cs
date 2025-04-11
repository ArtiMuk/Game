using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f; // Скорость героя
    [SerializeField] private float jumpForce = 0.61f; // Сила прыжка
    [SerializeField] private float dashForce = 30f;    // Сила рывка
    [SerializeField] private float dashCooldown = 0.8f; // Перезарядка рывка
    [SerializeField] private float dashDuration = 0.1f; // Длительность рывка


    private bool isOnGround = false; // Флаг, показывающий, что герой находится на поверхности
    private bool canDash = true; // Флаг для перезарядки рывка

    private Rigidbody2D body;
    private SpriteRenderer sprite;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Horizontal"))
            Run();
        if (isOnGround && Input.GetButton("Jump"))
            Jump();
        if (canDash && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Alpha8)))
            Dash();
    }

    private void FixedUpdate()
    {
        CheckIsOnGround();
    }

    private void Run()
    {
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

        sprite.flipX = direction.x < 0.0f; // Отражение спрайта при повороте налево
    }
    
    private void Jump()
    {
        body.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CheckIsOnGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isOnGround = collider.Length > 1;
    }

    private void Dash()
    {
        float direction = sprite.flipX ? -1f : 1f;
        body.linearVelocity = new Vector2(direction * dashForce, 0f); // Рывок осуществляется добавлением к вектору скорости силы рывка
        canDash = false;

        Invoke(nameof(StopDash), dashDuration);
        Invoke(nameof(EnableDash), dashCooldown);
    }

    private void StopDash() => body.linearVelocity = new Vector2(0f, body.linearVelocity.y);

    private void EnableDash() => canDash = true;

}
