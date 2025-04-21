using UnityEngine;

public class Hero : MonoBehaviour // Главный класс героя, наследуется от MonoBehaviour
{
    [SerializeField] private float speed = 5.0f; // Скорость передвижения героя
    [SerializeField] private float jumpForce = 13f; // Сила прыжка героя

    private bool isOnGround = false; // Стоит ли герой на земле
    private Rigidbody2D body;
    private SpriteRenderer sprite;

    private IAbility currentAbility; // Активная способность героя

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetButton("Horizontal")) // Проверка: нажата ли клавиша движения
            Run(); // Вызываем метод движения

        if (isOnGround && Input.GetButtonDown("Jump")) // Прыгаем, если стоим на земле и нажата клавиша прыжка
            Jump(); // Выполняем прыжок

        if (Input.GetKeyDown(KeyCode.Alpha1)) // Если нажата клавиша 1
        {
            ActivateWindAbility(); // Включаем способность ветра
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) // Если нажата клавиша 2
        {
            ActivateEarthAbility(); // Включаем способность Земли
        }

        currentAbility?.OnUpdate(); // Вызываем метод Update у способности, если она активна
    }

    private void FixedUpdate()
    {
        CheckIsOnGround(); // Проверяем, на земле ли герой
        currentAbility?.OnFixedUpdate(); // Вызываем FixedUpdate у способности, если она есть
    }

    private void Jump()
    {
        if (currentAbility != null) // Если есть активная способность
        {
            currentAbility.OnJump(); // Прыжок через способность
        }
        else // Иначе обычный прыжок
        {
            body.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Run()
    {
        Vector3 direction = transform.right * Input.GetAxis("Horizontal"); // Получаем направление движения
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime); // Перемещаем героя
        sprite.flipX = direction.x < 0.0f; // Отражаем спрайт влево, если идём налево
    }

    private void ActivateWindAbility() // Метод включения способности Ветра
    {
        if (currentAbility == null) // Если способность ещё не активна
        {
            var windAbility = gameObject.AddComponent<WindAbility>(); // Добавляем компонент WindAbility на объект
            windAbility.Init(body, sprite); // Инициализируем способность
            currentAbility = windAbility; // Сохраняем ссылку на активную способность
            Debug.Log("Способность ветра активирована!");
        }
    }

    private void ActivateEarthAbility() // Метод включения способности Земли
    {
        if (currentAbility == null) // Если способность ещё не активна
        {
            var earthAbility = gameObject.AddComponent<EarthAbility>(); // Добавляем компонент EarthAbility на объект
            earthAbility.Init(body, sprite); // Передаем jumpForce
            currentAbility = earthAbility; // Сохраняем ссылку на активную способность
            Debug.Log("Способность Земли активирована!");
        }
    }

    private void CheckIsOnGround() // Проверяем, стоит ли герой на земле
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3f); // Проверяем коллайдеры вокруг позиции героя
        bool wasGrounded = isOnGround; // Сохраняем старое состояние
        isOnGround = colliders.Length > 1; // Если больше одного коллайдера — герой стоит на земле

        if (!wasGrounded && isOnGround) // Если только что приземлился
        {
            currentAbility?.OnLand(); // Сообщаем способности о приземлении
        }
    }
}
