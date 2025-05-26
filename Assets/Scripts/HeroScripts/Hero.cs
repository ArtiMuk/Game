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
    private Animator animator;

    private AbilityManager abilityManager; // Менеджер способностей

    [Header("Ability Specific Assets")]
    [SerializeField] public Sprite waterPuddleSprite;

    [SerializeField] public GameObject fireCheckpointFlagPrefab;

    public enum HeroMode { Default, Wind, Earth, Fire, Water }
    private HeroMode currentMode = HeroMode.Default;

    private static readonly System.Collections.Generic.Dictionary<HeroMode, Color> modeColors = new()
    {
        { HeroMode.Default, Color.white },
        { HeroMode.Wind, Color.cyan },
        { HeroMode.Earth, new Color(0.55f, 0.27f, 0.07f) },
        { HeroMode.Fire, Color.red },
        { HeroMode.Water, new Color(0.2f, 0.5f, 1f) }
    };

    public void SetMode(HeroMode mode)
    {
        currentMode = mode;
        if (sprite != null && modeColors.ContainsKey(mode))
            sprite.color = modeColors[mode];
    }

    // Для вызова из AbilityManager
    public void SetWindMode() => SetMode(HeroMode.Wind);
    public void SetEarthMode() => SetMode(HeroMode.Earth);
    public void SetFireMode() => SetMode(HeroMode.Fire);
    public void SetWaterMode() => SetMode(HeroMode.Water);
    public void SetDefaultMode() => SetMode(HeroMode.Default);

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

        abilityManager = gameObject.AddComponent<AbilityManager>();
        abilityManager.Init(body, sprite);
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // Получаем ввод по горизонтали
        animator.SetFloat("Speed", Mathf.Abs(horizontalInput)); // Устанавливаем параметр "Speed" в Animator
        animator.SetFloat("VerticalVelocity", body.linearVelocity.y); // Передаем вертикальную скорость в Animator

        if (horizontalInput != 0) // Проверка: есть ли ввод по горизонтали
            Run(horizontalInput); // Вызываем метод движения, передаем ввод

        if (isOnGround && Input.GetButtonDown("Jump")) // Прыгаем, если стоим на земле и нажата клавиша прыжка
            Jump(); // Выполняем прыжок

        if (Input.GetKeyDown(KeyCode.Alpha1))
            abilityManager.SwitchToWindAbility();

        if (Input.GetKeyDown(KeyCode.Alpha2))
            abilityManager.SwitchToEarthAbility();

        if (Input.GetKeyDown(KeyCode.Alpha3))
            abilityManager.SwitchToFireAbility();

        if (Input.GetKeyDown(KeyCode.Alpha4))
            abilityManager.SwitchToWaterAbility();

        abilityManager.UpdateAbility();
    }

    private void FixedUpdate()
    {
        CheckIsOnGround(); // Проверяем, на земле ли герой
        animator.SetBool("IsGrounded", isOnGround); // Обновляем параметр IsGrounded в Animator
        abilityManager.FixedUpdateAbility();
    }

    private void Jump()
    {
        animator.SetTrigger("Jump"); // Активируем триггер Jump в Animator
        if (abilityManager != null && abilityManager.HasActiveAbility())
        {
            abilityManager.JumpAbility();
        }
        else
        {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Run(float horizontalInput) // Принимаем ввод как аргумент
    {
        Vector3 direction = transform.right * Mathf.Sign(horizontalInput); // Направление зависит от знака ввода

        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + Vector2.up * 0.5f, direction, 0.4f, LayerMask.GetMask("Wall", "Neidi")); //Проверяем что перед героем нет стены

        if (hit.collider == null)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime); // Перемещаем героя
        }
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