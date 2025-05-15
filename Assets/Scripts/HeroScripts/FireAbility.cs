using UnityEngine;

public class FireAbility : MonoBehaviour, IAbility
{
    private Rigidbody2D body;
    private SpriteRenderer sprite;
    private Hero hero;

    [SerializeField] private float jumpForce = 13f;              // Сила прыжка
    [SerializeField] private float speedBoostMultiplier = 2f;    // Во сколько раз спринт
    [SerializeField] private float boostDuration = 5f;           // Длительность спринта
    [SerializeField] private float cooldownDuration = 5f;        // Перезарядка спринта

    private float originalSpeed;   // Запомним скорость, чтобы вернуть
    private bool isBoosting;       // Флаг: ускорение активно
    private float boostTimer;      // Таймер ускорения
    private bool isOnCooldown;     // Флаг: идёт перезарядка
    private float cooldownTimer;   // Таймер перезарядки

    private Vector3 checkpoint; // текущий чекпойнт

    public void Init(Rigidbody2D rb, SpriteRenderer sr)
    {
        body = rb;
        sprite = sr;
        hero = rb.GetComponent<Hero>();      // Ссылка на скрипт Hero
        originalSpeed = hero.Speed;          // Запоминаем исходную скорость
        checkpoint = hero.transform.position; // стартовый
    }

    public void OnUpdate()
    {
        // Нажатие E запускает спринт один раз
        if (Input.GetKeyDown(KeyCode.E) && !isBoosting && !isOnCooldown)
        {
            ActivateBoost();
        }

        // Отсчёт времени спринта
        if (isBoosting)
        {
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0f)
            {
                DeactivateBoost();
                // после окончания спринта сразу стартует перезарядка
                isOnCooldown = true;
                cooldownTimer = cooldownDuration;
            }
        }
        // Отсчёт перезарядки
        else if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            checkpoint = hero.transform.position;
            Debug.Log($"[Fire] Checkpoint set at {checkpoint}");
        }

    }

    public void OnFixedUpdate() { }

    public void OnJump()
    {
        // Обычный прыжок
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
    }

    public void OnLand() { }

    public void OnExit()
    {
        if (isBoosting)
        {
            DeactivateBoost();
            isBoosting = false;
            isOnCooldown = true;
            cooldownTimer = cooldownDuration;
        }
    }


    private void ActivateBoost()
    {
        isBoosting = true;
        boostTimer = boostDuration;
        hero.Speed = originalSpeed * speedBoostMultiplier; // Удваиваем скорость бега
        Debug.Log("Fire sprint activated!");
    }

    private void DeactivateBoost()
    {
        isBoosting = false;
        hero.Speed = originalSpeed; // Возвращаем исходную скорость
        Debug.Log("Fire sprint ended.");
    }
    public Vector3 GetCheckpoint()
    {
        return checkpoint;
    }
}
