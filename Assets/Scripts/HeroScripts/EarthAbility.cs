using UnityEngine;

public class EarthAbility : MonoBehaviour, IAbility
{
    private Rigidbody2D body;
    private SpriteRenderer sprite;
    private Camera mainCamera; // Ссылка на главную камеру для управления её положением (тряска)

    [SerializeField] private float fastFallMultiplier = 3.0f; // Множитель гравитации при ускоренном падении
    [SerializeField] private float jumpForce = 13f; // Сила прыжка вверх
    [SerializeField] private float shakeIntensity = 0.05f; // Интенсивность тряски камеры
    [SerializeField] private float shakeDuration = 0.5f; // Длительность тряски камеры в секундах

    private float originalGravityScale; // Оригинальное значение гравитации, чтобы можно было возвращать после ускорения
    private bool isShaking = false; // Флаг, включена ли сейчас тряска камеры
    private float shakeTimer = 0f; // Таймер обратного отсчёта для тряски камеры
    private Vector3 originalCameraPosition; // Исходная позиция камеры до тряски
    private bool isFastFalling = false; // Флаг, падает ли игрок ускоренно
    private bool wasGroundedLastFrame = false; // Был ли игрок на земле в предыдущем кадре

    public void Init(Rigidbody2D rb, SpriteRenderer sr)
    {
        body = rb;
        sprite = sr;
        mainCamera = Camera.main; // Получаем основную камеру
        originalGravityScale = body.gravityScale; // Запоминаем исходную силу гравитации
        originalCameraPosition = mainCamera.transform.position; // Сохраняем исходную позицию камеры
    }

    public void OnUpdate()
    {
        originalCameraPosition = mainCamera.transform.position; // Обновляем позицию камеры каждый кадр

        HandleFastFall(); // Обработка логики ускоренного падения
        HandleLandingShake(); // Проверка, нужно ли активировать тряску при приземлении
        HandleCameraShake(); // Если активна тряска — обновляем её поведение
    }

    public void OnFixedUpdate() { } 

    public void OnJump()
    {
        PerformJump(); // Запускаем метод прыжка
    }

    public void OnLand() 
    {
        StopFastFall(); // Отключаем ускоренное падение
    }

    private void HandleFastFall() // Обработка ускоренного падения
    {
        if (Input.GetKey(KeyCode.RightShift)) // Если зажата клавиша правый Shift
        {
            body.gravityScale = originalGravityScale * fastFallMultiplier; // Увеличиваем гравитацию
            isFastFalling = true; // Помечаем, что игрок ускоренно падает
        }
        else
        {
            body.gravityScale = originalGravityScale; // Возвращаем нормальную гравитацию
        }
    }

    private void HandleLandingShake() // Проверка на приземление после ускоренного падения
    {
        bool grounded = IsGrounded(); // Проверяем, на земле ли игрок

        if (grounded && !wasGroundedLastFrame && isFastFalling) // Если только что приземлились с ускоренного падения
        {
            TriggerShake(); // Запускаем тряску
            isFastFalling = false; // Выключаем флаг ускоренного падения
        }

        wasGroundedLastFrame = grounded; // Запоминаем текущее состояние на земле
    }

    private void HandleCameraShake() // Обновление тряски камеры
    {
        if (!isShaking) return; // Если тряска не активна — ничего не делаем

        shakeTimer -= Time.deltaTime; // Уменьшаем таймер на время кадра

        if (shakeTimer > 0)
        {
            // Смещаем камеру случайным образом на каждый кадр в пределах shakeIntensity
            mainCamera.transform.position = originalCameraPosition + Random.insideUnitSphere * shakeIntensity;
        }
        else
        {
            StopShake(); // Завершаем тряску
        }
    }

    private void PerformJump() // Прыжок вверх
    {
        Debug.Log("Earth Jump!"); // Сообщение в консоль
        body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Добавляем импульс вверх
        isFastFalling = false; // Отключаем ускоренное падение
    }

    private void StopFastFall() // Остановка ускоренного падения
    {
        if (isFastFalling) // Если оно было активно
        {
            isFastFalling = false; // Выключаем
        }
    }

    private void TriggerShake() // Запуск тряски камеры
    {
        isShaking = true; // Включаем флаг
        shakeTimer = shakeDuration; // Устанавливаем таймер на длительность тряски
    }

    private void StopShake() // Остановка тряски камеры
    {
        isShaking = false; // Выключаем флаг
        mainCamera.transform.position = originalCameraPosition; // Возвращаем камеру в исходное положение
    }

    private bool IsGrounded() // Проверка, стоит ли игрок на земле
    {
        return Physics2D.OverlapCircleAll(transform.position, 0.3f).Length > 1;
    }
}
