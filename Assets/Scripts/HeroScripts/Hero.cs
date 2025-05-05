using UnityEngine;

public class Hero : MonoBehaviour // Главный класс героя, наследуется от MonoBehaviour
{
    [SerializeField] private float speed = 5.0f; // Скорость передвижения героя
    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    [SerializeField] private float jumpForce = 13f; // Сила прыжка героя
    private bool isOnGround = false; // Стоит ли герой на земле
    private Rigidbody2D body;
    private SpriteRenderer sprite;

    private AbilityManager abilityManager; // Менеджер способностей

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        abilityManager = gameObject.AddComponent<AbilityManager>();
        abilityManager.Init(body, sprite);
    }

    private void Update()
    {
        if (Input.GetButton("Horizontal")) // Проверка: нажата ли клавиша движения
            Run(); // Вызываем метод движения

        if (isOnGround && Input.GetButtonDown("Jump")) // Прыгаем, если стоим на земле и нажата клавиша прыжка
            Jump(); // Выполняем прыжок

        if (Input.GetKeyDown(KeyCode.Alpha1))
            abilityManager.SwitchToWindAbility();

        if (Input.GetKeyDown(KeyCode.Alpha2))
            abilityManager.SwitchToEarthAbility();

        if (Input.GetKeyDown(KeyCode.Alpha3))
            abilityManager.SwitchToFireAbility();

        abilityManager.UpdateAbility();
    }

    private void FixedUpdate()
    {
        CheckIsOnGround(); // Проверяем, на земле ли герой
        abilityManager.FixedUpdateAbility();
    }

    private void Jump()
    {
        if (abilityManager != null && abilityManager.HasActiveAbility())
        {
            abilityManager.JumpAbility();
        }
        else
        {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }


    private void Run()
    {
        Vector3 direction = transform.right * Input.GetAxis("Horizontal"); // Получаем направление движения

        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + Vector2.up * 0.5f, direction, 0.4f, LayerMask.GetMask("Wall", "Neidi")); //Проверяем что перед героем нет стены

        if (hit.collider == null)
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime); // Перемещаем героя
        else
            body.linearVelocity = new Vector2(0, body.linearVelocity.y);

        sprite.flipX = direction.x < 0.0f; // Отражаем спрайт влево, если идём налево
    }

    private void CheckIsOnGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        bool wasGrounded = isOnGround;
        isOnGround = colliders.Length > 1;

        if (!wasGrounded && isOnGround)
        {
            abilityManager.LandAbility();
            // При необходимости способности обрабатывают OnLand внутри себя
        }
    }

    // Для внешнего доступа к состоянию "на земле"
    public bool IsOnGround() => isOnGround;
}
