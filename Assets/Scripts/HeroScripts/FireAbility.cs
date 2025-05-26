using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FireAbility : MonoBehaviour, IAbility
{
    private static int totalCheckpoints = 0;
    private static List<Vector3> savedCheckpoints = new List<Vector3>();
    private static GameObject currentFlag; // Делаем флаг статическим
    private static int currentSceneIndex = -1; // Индекс текущей сцены
    private Rigidbody2D body;
    private SpriteRenderer sprite;

    private GameObject flagPrefab;
    private Hero hero;

    [SerializeField] private float jumpForce = 13f;
    [SerializeField] private float speedBoostMultiplier = 3.5f;
    [SerializeField] private float boostDuration = 5f;
    [SerializeField] private float cooldownDuration = 3f;

    private float originalSpeed;
    private bool isBoosting;
    private float boostTimer;
    private bool isOnCooldown;
    private float cooldownTimer;

    private List<Vector3> checkpoints = new List<Vector3>(3);

    private void OnEnable()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        // Сбрасываем чекпоинты только если загружена новая сцена
        if (sceneIndex != currentSceneIndex)
        {
            currentSceneIndex = sceneIndex;
            totalCheckpoints = 0;
            savedCheckpoints.Clear();
            if (currentFlag != null)
            {
                Destroy(currentFlag);
                currentFlag = null;
            }
            Debug.Log($"[Fire] Новая сцена загружена, чекпоинты сброшены. Сцена: {sceneIndex}");
        }
    }

    public void Init(Rigidbody2D rb, SpriteRenderer sr)
    {
        body = rb;
        sprite = sr;
        hero = rb.GetComponent<Hero>();
        originalSpeed = hero.Speed;

        // Восстанавливаем сохраненные чекпоинты
        checkpoints.Clear();
        if (savedCheckpoints.Count > 0)
        {
            checkpoints.AddRange(savedCheckpoints);
            // Восстанавливаем флаг на последнем чекпоинте, если его еще нет
            if (currentFlag == null)
            {
                PlaceFlagAt(checkpoints[checkpoints.Count - 1]);
            }
        }
        else
        {
            // Стартовый чекпойнт (позиция старта уровня)
            GameObject startCheck = GameObject.FindGameObjectWithTag("CheckForFire");
            checkpoints.Add(startCheck.transform.position);
            if (currentFlag == null)
            {
                PlaceFlagAt(hero.transform.position);
            }
        }
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
            if (totalCheckpoints >= 2)
            {
                Debug.Log("[Fire] Максимум 2 чекпойнта установлено. Новые чекпойнты недоступны.");
                return;
            }

            Vector3 newCheckpoint = hero.transform.position;
            checkpoints.Add(newCheckpoint);
            totalCheckpoints++;
            Debug.Log($"[Fire] Checkpoint set at {newCheckpoint}. Всего чекпоинтов: {totalCheckpoints}");

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

        // Сохраняем чекпоинты при выходе
        savedCheckpoints.Clear();
        savedCheckpoints.AddRange(checkpoints);
        // Не удаляем флаг при выходе
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

    // Метод для сброса чекпоинтов (можно вызвать при смерти героя)
    public static void ResetCheckpoints()
    {
        totalCheckpoints = 0;
        savedCheckpoints.Clear();
        if (currentFlag != null)
        {
            Destroy(currentFlag);
            currentFlag = null;
        }
    }
}
