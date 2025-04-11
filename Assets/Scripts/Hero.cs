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
        if (Input.GetButton("Horizontal")) // A,D и стрелки для передвижения (не указаны явно, перетянуты из Unity)
            Run();
        if (isOnGround && Input.GetButton("Jump"))
            Jump();
        if (canDash && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Alpha8))) // Лкм и 8 для рывка
            Dash();
    }

    private void FixedUpdate()
    {
        CheckIsOnGround();
    }

    private void Run()
    {
        // Создаем вектор направления движения:
        // - transform.right - ось X (1,0,0) в локальных координатах объекта
        // - Input.GetAxis("Horizontal") возвращает значение от -1 (влево) до 1 (вправо)
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");

        // Плавно перемещаем персонажа:
        // - MoveTowards перемещает объект из текущей позиции (transform.position)
        // - в новую позицию (transform.position + direction)
        // - со скоростью speed * Time.deltaTime
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

        // Отражаем спрайт по горизонтали при движении влево:
        // - flipX = true, если direction.x отрицательный (движение влево)
        // - flipX = false, если direction.x положительный (движение вправо)
        sprite.flipX = direction.x < 0.0f;
    }

    private void Jump()
    {
        // Применяем импульсную силу вверх:
        // - transform.up - ось Y (0,1,0) в локальных координатах
        // - jumpForce - величина силы прыжка
        // - ForceMode2D.Impulse - придание импульса
        body.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CheckIsOnGround()
    {
        // Создаём массив коллайдеров, которые пересекаются с воображаемым кругом:
        // - transform.position - центр круга (текущая позиция объекта (позиция перетянута на ноги героя немного костыльно, в будущем лучше исправить))
        // - 0.3f - радиус круга проверки
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.3f); 
        isOnGround = collider.Length > 1; // OverlapCircleAll возвращает ВСЕ коллайдеры, включая собственный коллайдер объекта, поэтому >1
    }

    private void Dash()
    {
        // Чекаем направление рывка по отображению спрайта (дабы можно было совершать рывок с места, а не только при движении)
        float direction = sprite.flipX ? -1f : 1f; 
        body.linearVelocity = new Vector2(direction * dashForce, 0f); // Рывок осуществляется добавлением к вектору скорости вектора силы рывка с направлением direction
        canDash = false;

        Invoke(nameof(StopDash), dashDuration); // Обнуляем скорость для остановки рывка после dashDuration секунд
        Invoke(nameof(EnableDash), dashCooldown); // Возвращаем возможность рывка после окончания перезарядки
    }

    private void StopDash() => body.linearVelocity = new Vector2(0f, body.linearVelocity.y);

    private void EnableDash() => canDash = true;

}
