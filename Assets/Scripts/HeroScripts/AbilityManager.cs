using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private IAbility currentAbility;
    private Rigidbody2D body;
    private SpriteRenderer sprite;
    private Color defaultColor = Color.white;

    // Cache references to pre-attached ability components
    private EarthAbility earthAbilityComponent;
    private WindAbility windAbilityComponent;
    private FireAbility fireAbilityComponent;
    private WaterAbility waterAbilityComponent;

    public void Init(Rigidbody2D rb, SpriteRenderer sr)
    {
        body = rb;
        sprite = sr;
        if (sprite != null)
        {
            defaultColor = sprite.color;
        }
        else
        {
             Debug.LogError("SpriteRenderer is not assigned in AbilityManager Init.");
        }

        // Get references to the ability components on this GameObject
        earthAbilityComponent = GetComponent<EarthAbility>();
        windAbilityComponent = GetComponent<WindAbility>();
        fireAbilityComponent = GetComponent<FireAbility>();
        waterAbilityComponent = GetComponent<WaterAbility>();

        // Call Init for all pre-attached components once at the start with the correct Rigidbody and SpriteRenderer
        if (earthAbilityComponent) { earthAbilityComponent.Init(body, sprite); earthAbilityComponent.enabled = false; }
        if (windAbilityComponent) { windAbilityComponent.Init(body, sprite); windAbilityComponent.enabled = false; }
        if (fireAbilityComponent) { fireAbilityComponent.Init(body, sprite); fireAbilityComponent.enabled = false; }
        if (waterAbilityComponent) { waterAbilityComponent.Init(body, sprite); waterAbilityComponent.enabled = false; }
    }

    private void SetAbility(IAbility ability, MonoBehaviour abilityMonoBehaviour)
    {
        RemoveCurrentAbility();
        currentAbility = ability;
        if (abilityMonoBehaviour != null) 
        {
            abilityMonoBehaviour.enabled = true;
            // Init is now called once at the start for pre-attached components.
            // If re-initialization is needed on every switch, uncomment the line below.
            // currentAbility.Init(body, sprite); 
        }
        
        // Apply color based on the type, assuming AbilityColor is a property of the interface or specific types
        if (ability is WaterAbility water) ApplyAbilityColor(water.AbilityColor);
        else if (ability is FireAbility fire) ApplyAbilityColor(fire.AbilityColor);
        else if (ability is EarthAbility earth) ApplyAbilityColor(earth.AbilityColor);
        else if (ability is WindAbility wind) ApplyAbilityColor(wind.AbilityColor);
        else ApplyAbilityColor(defaultColor); // Fallback if type unknown or no specific color
    }

    public void SwitchToEarthAbility()
    {
        if (currentAbility is EarthAbility || !earthAbilityComponent) return;
        // earthAbilityComponent.Init(body, sprite); // Init called at start
        SetAbility(earthAbilityComponent, earthAbilityComponent);
        Debug.Log("Switched to Earth");
    }

    public void SwitchToWindAbility()
    {
        if (currentAbility is WindAbility || !windAbilityComponent) return;
        // windAbilityComponent.Init(body, sprite); // Init called at start
        SetAbility(windAbilityComponent, windAbilityComponent);
        Debug.Log("Switched to Wind");
    }

    public void SwitchToFireAbility()
    {
        if (currentAbility is FireAbility || !fireAbilityComponent) return;
        // fireAbilityComponent.Init(body, sprite); // Init called at start
        SetAbility(fireAbilityComponent, fireAbilityComponent);
        Debug.Log("Switched to Fire");
    }

<<<<<<< Updated upstream
=======
    public void SwitchToWaterAbility()
    {
        if (currentAbility is WaterAbility || !waterAbilityComponent) return;
        // waterAbilityComponent.Init(body, sprite); // Init called at start
        SetAbility(waterAbilityComponent, waterAbilityComponent);
        Debug.Log("Switched to Water");
    }

>>>>>>> Stashed changes
    private void RemoveCurrentAbility()
    {
        if (currentAbility != null)
        {
<<<<<<< Updated upstream
            Destroy(currentAbility as MonoBehaviour);
=======
            currentAbility.OnExit();
            if (currentAbility is MonoBehaviour currentMonoBehaviour)
            {
                currentMonoBehaviour.enabled = false;
            }
>>>>>>> Stashed changes
            currentAbility = null;
            ApplyAbilityColor(defaultColor);
        }
    }

    private void ApplyAbilityColor(Color abilityColor)
    {
        if (sprite != null)
        {
            sprite.color = abilityColor;
        }
        else
        {
            Debug.LogError("SpriteRenderer is not assigned in AbilityManager ApplyAbilityColor.");
        }
    }

    public void UpdateAbility()
    {
        // Only call OnUpdate if the component is enabled and is the current ability
        if (currentAbility != null && (currentAbility as MonoBehaviour).enabled) 
        {
            currentAbility.OnUpdate();
        }
    }

    public void FixedUpdateAbility()
    {
        if (currentAbility != null && (currentAbility as MonoBehaviour).enabled) 
        {
            currentAbility.OnFixedUpdate();
        }
    }

    public void JumpAbility()
    { 
        if (currentAbility != null && (currentAbility as MonoBehaviour).enabled) 
        {
            currentAbility.OnJump();
        }
    }

    public void LandAbility()
    {
        if (currentAbility != null && (currentAbility as MonoBehaviour).enabled) 
        {
            currentAbility.OnLand();
        }
    }

    public bool HasActiveAbility()
    {
        return currentAbility != null && (currentAbility as MonoBehaviour).enabled;
    }
}
