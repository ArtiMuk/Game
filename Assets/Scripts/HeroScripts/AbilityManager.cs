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

    public void SwitchToEarthAbility()
    {
        if (currentAbility is EarthAbility)
            return;

        RemoveCurrentAbility();

        var earthAbility = gameObject.AddComponent<EarthAbility>();
        earthAbility.Init(body, sprite);
        earthAbility.SetWallLayer(LayerMask.GetMask("Wall"));
        currentAbility = earthAbility;

        hero?.SetEarthMode();
        Debug.Log("Переключение на режим земли");
    }

    public void SwitchToWindAbility()
    {
        if (currentAbility is WindAbility)
            return;

        RemoveCurrentAbility();

        var windAbility = gameObject.AddComponent<WindAbility>();
        windAbility.Init(body, sprite);
        currentAbility = windAbility;

        hero?.SetWindMode();
        Debug.Log("Переключение на режим ветра");
    }

    public void SwitchToFireAbility()
    {
        if (currentAbility is FireAbility)
            return;

        RemoveCurrentAbility();

        var fireAbility = gameObject.AddComponent<FireAbility>();
        fireAbility.Init(body, sprite);
        currentAbility = fireAbility;

        hero?.SetFireMode();
        Debug.Log("Переключение на режим огня");
    }

    public void SwitchToWaterAbility()
    {
        if (currentAbility is WaterAbility)
            return;

        RemoveCurrentAbility();

        var waterAbility = gameObject.AddComponent<WaterAbility>();
        waterAbility.Init(body, sprite);
        currentAbility = waterAbility;

        hero?.SetWaterMode();
        Debug.Log("Переключение на режим воды");
    }


    private void RemoveCurrentAbility()
    {
        if (currentAbility != null)
        {
            currentAbility.OnExit();
            Destroy(currentAbility as MonoBehaviour);
            currentAbility = null;
        }
    }

    public void UpdateAbility()
    {
        currentAbility?.OnUpdate();
    }

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
