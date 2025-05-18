using System.Collections.Generic;
using UnityEngine;

public class FireAbility : MonoBehaviour, IAbility
{
    private Rigidbody2D body;
    private SpriteRenderer sprite;

    private GameObject flagPrefab;
    private GameObject currentFlag;
    private Hero hero;

    [SerializeField] private float jumpForce = 13f;
    [SerializeField] private float speedBoostMultiplier = 2f;
    [SerializeField] private float boostDuration = 5f;
    [SerializeField] private float cooldownDuration = 5f;

    private float originalSpeed;
    private bool isBoosting;
    private float boostTimer;
    private bool isOnCooldown;
    private float cooldownTimer;

    private List<Vector3> checkpoints = new List<Vector3>(3);

    public void Init(Rigidbody2D rb, SpriteRenderer sr)
    {
        body = rb;
        sprite = sr;
        hero = rb.GetComponent<Hero>();
        originalSpeed = hero.Speed;

        // Стартовый чекпойнт (позиция старта уровня)
        checkpoints.Add(hero.transform.position);

        PlaceFlagAt(hero.transform.position);
    }

    public void SetFlagPrefab(GameObject prefab)
    {
        flagPrefab = prefab;
    }

    public void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isBoosting && !isOnCooldown)
        {
            ActivateBoost();
        }

        if (isBoosting)
        {
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0f)
            {
                DeactivateBoost();
                isOnCooldown = true;
                cooldownTimer = cooldownDuration;
            }
        }
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
            if (checkpoints.Count >= 3)
            {
                Debug.Log("[Fire] Максимум 2 чекпойнта установлено. Новые чекпойнты недоступны.");
                return;
            }

            Vector3 newCheckpoint = hero.transform.position;
            checkpoints.Add(newCheckpoint);
            Debug.Log($"[Fire] Checkpoint set at {newCheckpoint}");

            PlaceFlagAt(newCheckpoint);
        }
    }

    public void OnFixedUpdate() { }

    public void OnJump()
    {
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
        hero.Speed = originalSpeed * speedBoostMultiplier;
        Debug.Log("Fire sprint activated!");
    }

    private void DeactivateBoost()
    {
        isBoosting = false;
        hero.Speed = originalSpeed;
        Debug.Log("Fire sprint ended.");
    }

    public Vector3 GetCheckpoint()
    {
        // Возвращаем последний чекпойнт (последний в списке)
        if (checkpoints.Count == 0)
            return hero.transform.position;

        return checkpoints[checkpoints.Count - 1];
    }

    private void PlaceFlagAt(Vector3 position)
    {
        if (flagPrefab == null)
        {
            Debug.LogWarning("[Fire] Флаг-префаб не установлен.");
            return;
        }

        if (currentFlag != null)
        {
            Destroy(currentFlag);
        }

        currentFlag = Instantiate(flagPrefab, position + new Vector3(0f, 0.4f, 0f), Quaternion.identity);
        Debug.Log($"[Fire] Флаг установлен на позиции {position}");
    }
}
