using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public IAbility currentAbility;
    private Rigidbody2D body;
    private SpriteRenderer sprite;
    private Hero hero;

    public void Init(Rigidbody2D rb, SpriteRenderer sr)
    {
        body = rb;
        sprite = sr;
        hero = GetComponent<Hero>();
    }

    // Переключение на земляную способность
    public void SwitchToEarthAbility()
    {
        // Проверка, находится ли водная способность в форме лужи
        if (currentAbility is WaterAbility waterPuddle && waterPuddle.IsInPuddleForm())
        {
            Debug.Log("Cannot switch ability: Currently in puddle form.");
            return;
        }
        if (currentAbility is EarthAbility)
            return;

        RemoveCurrentAbility();

        // Создаем и инициализируем земляную способность
        var earthAbility = gameObject.AddComponent<EarthAbility>();
        earthAbility.Init(body, sprite);
        earthAbility.SetWallLayer(LayerMask.GetMask("Wall"));
        currentAbility = earthAbility;

        hero?.SetEarthMode();
        Debug.Log("Переключение на режим земли");
    }

    // Переключение на ветряную способность
    public void SwitchToWindAbility()
    {
        if (currentAbility is WaterAbility waterPuddle && waterPuddle.IsInPuddleForm())
        {
            Debug.Log("Cannot switch ability: Currently in puddle form.");
            return;
        }
        if (currentAbility is WindAbility)
            return;

        RemoveCurrentAbility();

        var windAbility = gameObject.AddComponent<WindAbility>();
        windAbility.Init(body, sprite);
        currentAbility = windAbility;

        hero?.SetWindMode();
        Debug.Log("Переключение на режим ветра");
    }

    // Переключение на огненную способность
    public void SwitchToFireAbility()
    {
        if (currentAbility is WaterAbility waterPuddle && waterPuddle.IsInPuddleForm())
        {
            Debug.Log("Cannot switch ability: Currently in puddle form.");
            return;
        }
        if (currentAbility is FireAbility)
            return;

        RemoveCurrentAbility();

        var fireAbility = gameObject.AddComponent<FireAbility>();
        fireAbility.Init(body, sprite);

        // Устанавливаем префаб флажка для создания контрольных точек
        if (hero != null && hero.fireCheckpointFlagPrefab != null)
        {
            fireAbility.SetFlagPrefab(hero.fireCheckpointFlagPrefab);
        }
        else
        {
            Debug.LogError("AbilityManager: Hero component or hero.fireCheckpointFlagPrefab is null. Cannot set flag prefab for FireAbility.");
        }

        currentAbility = fireAbility;

        hero?.SetFireMode();
        Debug.Log("Переключение на режим огня");
    }

    // Переключение на водную способность
    public void SwitchToWaterAbility()
    {
        if (currentAbility is WaterAbility)
            return;

        RemoveCurrentAbility();

        var waterAbility = gameObject.AddComponent<WaterAbility>();
        waterAbility.Init(body, sprite);
        // Устанавливаем спрайт для формы лужи
        if (hero != null && hero.waterPuddleSprite != null)
        {
            waterAbility.SetPuddleSprite(hero.waterPuddleSprite);
        }
        else
        {
            Debug.LogError("AbilityManager: Hero component or hero.waterPuddleSprite is null. Cannot set puddle sprite for WaterAbility.");
        }
        currentAbility = waterAbility;

        hero?.SetWaterMode();
        Debug.Log("Переключение на режим воды");
    }

    // Удаление текущей способности
    private void RemoveCurrentAbility()
    {
        if (currentAbility != null)
        {
            currentAbility.OnExit();
            Destroy(currentAbility as MonoBehaviour);
            currentAbility = null;
        }
    }

    // Обновление способности каждый кадр
    public void UpdateAbility()
    {
        currentAbility?.OnUpdate();
    }

    // Обновление способности в фиксированном кадре
    public void FixedUpdateAbility()
    {
        currentAbility?.OnFixedUpdate();
    }

    public void JumpAbility()
    {
        currentAbility?.OnJump();
    }

    public void LandAbility()
    {
        currentAbility?.OnLand();
    }

    public bool HasActiveAbility()
    {
        return currentAbility != null;
    }
}
