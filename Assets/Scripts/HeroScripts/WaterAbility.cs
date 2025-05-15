using UnityEngine;

public class WaterAbility : MonoBehaviour, IAbility
{
    private Rigidbody2D body;
    private SpriteRenderer sprite;
    private Hero hero;
    private BoxCollider2D boxCollider;
    private Animator animator;

    [SerializeField] private float jumpForce = 13f;
    [SerializeField] private Vector2 normalColliderSize = new Vector2(0.7442487f, 0.9808896f);
    [SerializeField] private Vector2 normalOffset = new Vector2(4.172325e-07f, 0.4589568f);
    [SerializeField] private Vector2 puddleColliderSize = new Vector2(0.7442487f, 0.2f);
    [SerializeField] private Vector2 puddleOffset = new Vector2(4.172325e-07f, 0.068512f);
    [SerializeField] private Sprite puddleSprite;
    private Sprite normalSprite;

    private bool isPuddle = false;

    public Color AbilityColor { get; } = Color.blue;

    public void Init(Rigidbody2D rb, SpriteRenderer sr)
    {
        body = rb;
        sprite = sr;

        if (sprite != null)
        {
            normalSprite = sprite.sprite;
            animator = sprite.GetComponent<Animator>();
            if (animator == null)
            {
                 Debug.LogWarning("[WaterAbility Init] Animator not found on " + sprite.gameObject.name + ". Puddle form animations won't be disabled.");
            }
        }
        else
        {
            Debug.LogError("WaterAbility: SpriteRenderer is null during Init.");
        }

        hero = rb.GetComponentInParent<Hero>(); 
        if (hero == null) 
        {
            Debug.LogError("WaterAbility: Hero component not found on Rigidbody's GameObject or its parents.");
        }
        else
        {
            boxCollider = hero.GetComponent<BoxCollider2D>();
            if (boxCollider == null) 
            {
                Debug.LogError("WaterAbility: BoxCollider2D not found on Hero.");
            }
        }

        if (puddleSprite == null)
        {
            Debug.LogWarning("WaterAbility: Puddle Sprite is not assigned in the Inspector. Puddle form will not change sprite.");
        }
    }

    public void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            EnterPuddleForm();
        }
        else if (Input.GetKeyUp(KeyCode.RightShift))
        {
            ExitPuddleForm();
        }
    }

    public void OnFixedUpdate() { }

    public void OnJump()
    {
        if (!isPuddle && body != null)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        }
    }

    public void OnLand() { }

    public void OnExit()
    {
        if (isPuddle)
        {
            ExitPuddleForm(); 
        }
    }

    private void EnterPuddleForm()
    {
        if (isPuddle) return;
        
        if (puddleSprite == null) 
        {
            Debug.LogWarning("WaterAbility: Cannot enter puddle form - Puddle Sprite is not assigned.");
            return; // Don't change to puddle form if no sprite is assigned
        }
        if (sprite == null) 
        {
            Debug.LogError("WaterAbility: SpriteRenderer reference is null. Cannot change sprite.");
            return;
        }
        if (boxCollider == null)
        {
            Debug.LogError("WaterAbility: BoxCollider2D reference is null. Cannot change collider.");
            return;
        }

        if (animator != null) animator.enabled = false;
        
        isPuddle = true;
        boxCollider.size = puddleColliderSize;
        boxCollider.offset = puddleOffset;
        sprite.sprite = puddleSprite; 
    }

    private void ExitPuddleForm()
    {
        if (!isPuddle) return;

        if (sprite == null) 
        {
            Debug.LogError("WaterAbility: SpriteRenderer reference is null. Cannot revert sprite.");
            // Attempt to re-enable animator anyway if it exists
            if (animator != null) animator.enabled = true;
            return; 
        }
         if (boxCollider == null)
        {
            Debug.LogError("WaterAbility: BoxCollider2D reference is null. Cannot revert collider.");
            // Attempt to re-enable animator and revert sprite if possible
        }
        else
        {
            boxCollider.size = normalColliderSize;
            boxCollider.offset = normalOffset;
        }

        if (animator != null) animator.enabled = true;

        // Revert to normal sprite only if it was cached
        // If normalSprite is null for some reason, it might be better to leave the puddle sprite
        // or have a default fallback, but for now, we revert if possible.
        if (normalSprite != null) 
        {
            sprite.sprite = normalSprite; 
        }
        else
        {
            Debug.LogWarning("WaterAbility: normalSprite was null, cannot revert to it.");
        }
        isPuddle = false;
    }
}

