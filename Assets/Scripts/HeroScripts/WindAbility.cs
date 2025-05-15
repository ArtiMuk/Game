using UnityEngine;

public class WindAbility : MonoBehaviour, IAbility // Класс способности ветер, реализует интерфейс IAbility
{
    private Rigidbody2D body;
    private SpriteRenderer sprite;

    private bool isHoldingJump; // Зажата ли клавиша прыжка
    private bool hasDoubleJumped = false; // Был ли уже выполнен двойной прыжок

    [SerializeField] private float fallSlowFactor = 0.2f; // Множитель замедления падения при планировании
    [SerializeField] private float jumpForce = 13f; // Сила прыжка
    [SerializeField] private float dashForce = 30f; // Сила рывка
    [SerializeField] private float dashCooldown = 0.8f; // Время перезарядки рывка
    [SerializeField] private float dashDuration = 0.1f; // Продолжительность рывка

    private bool canDash = true; // можно ли сейчас сделать рывок

    public void Init(Rigidbody2D rb, SpriteRenderer sr)
    {
        body = rb;
        sprite = sr;
    }

    public void OnUpdate()
    {
        isHoldingJump = Input.GetButton("Jump"); // Проверяем, зажата ли клавиша прыжка
        HandleDash(); // Обработка рывка
        HandleDoubleJump(); // Обработка двойного прыжка
    }

    public void OnFixedUpdate()
    {
        HandleGlide(); // Обработка планирования
    }

    public void OnJump() // Вызывается при обычном прыжке
    {
        PerformJump(); // Выполняем обычный прыжок и сбрасываем двойной
    }

    public void OnExit() { }
    private void PerformJump() // Метод обычного прыжка
    {
        Debug.Log("Wind Jump!");
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        hasDoubleJumped = false; // Сбрасываем флаг двойного прыжка
    }

    private void HandleDoubleJump() // Обработка второго прыжка
    {
        if (!hasDoubleJumped && !IsGrounded() && Input.GetButtonDown("Jump")) // Если не прыгал дважды и в воздухе
        {
            Debug.Log("Double Jump!");
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce); // Второй прыжок вверх
            hasDoubleJumped = true; // Помечаем, что двойной прыжок выполнен
        }
    }

    private void HandleDash() // Обработка рывка
    {
        if (canDash && Input.GetKeyDown(KeyCode.E)) // Если рывок доступен и нажата клавиша E
        {
            float direction = sprite.flipX ? -1f : 1f; // Определяем направление в зависимости от направления спрайта
            body.linearVelocity = new Vector2(direction * dashForce, 0f); // Устанавливаем горизонтальную скорость рывка
            canDash = false; // Блокируем рывок
            Invoke(nameof(StopDash), dashDuration); // Через время dashDuration остановим рывок
            Invoke(nameof(EnableDash), dashCooldown); // Через время dashCooldown снова разрешим рывок
        }
    }

    private void HandleGlide() // Обработка планирования
    {
        if (Input.GetKey(KeyCode.RightShift) && body.linearVelocity.y < 0) // Если зажат Shift и герой падает
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, body.linearVelocity.y * fallSlowFactor); // Замедляем падение
        }
    }

    private void StopDash() => body.linearVelocity = new Vector2(0f, body.linearVelocity.y); // Остановка рывка по горизонтали

    private void EnableDash() => canDash = true; // Разрешаем новый рывок

    public void OnLand() => hasDoubleJumped = false; // При приземлении сбрасываем возможность двойного прыжка

    private bool IsGrounded() // Проверка, стоит ли персонаж на земле
    {
        return Physics2D.OverlapCircleAll(transform.position, 0.3f).Length > 1; // Если вокруг коллайдеров больше одного (включая себя) — значит на земле
    }
}
