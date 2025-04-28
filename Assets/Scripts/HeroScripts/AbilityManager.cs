using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private IAbility currentAbility;
    private Rigidbody2D body;
    private SpriteRenderer sprite;

    public void Init(Rigidbody2D rb, SpriteRenderer sr)
    {
        body = rb;
        sprite = sr;
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

        Debug.Log("Переключено на Землю");
    }

    public void SwitchToWindAbility()
    {
        if (currentAbility is WindAbility)
            return;

        RemoveCurrentAbility();

        var windAbility = gameObject.AddComponent<WindAbility>();
        windAbility.Init(body, sprite);
        currentAbility = windAbility;

        Debug.Log("Переключено на Ветер");
    }

    public void SwitchToFireAbility()
    {
        if (currentAbility is FireAbility)
            return;

        RemoveCurrentAbility();

        var fireAbility = gameObject.AddComponent<FireAbility>();
        fireAbility.Init(body, sprite);
        currentAbility = fireAbility;

        Debug.Log("Переключено на Огонь");
    }

    private void RemoveCurrentAbility()
    {
        if (currentAbility != null)
        {
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
