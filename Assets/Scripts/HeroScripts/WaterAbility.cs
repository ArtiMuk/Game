using UnityEngine;

public class WaterAbility : MonoBehaviour, IAbility
{
    private Rigidbody2D body;
    private SpriteRenderer sprite;
    private Hero hero;
    private BoxCollider2D boxCollider; // �������� CapsuleCollider2D �� BoxCollider2D
    private Animator animator;

    [SerializeField] private float jumpForce = 13f;
    [SerializeField] private Vector2 normalColliderSize = new Vector2(0.7442487f, 0.9808896f); // �� Unity
    [SerializeField] private Vector2 normalOffset = new Vector2(4.172325e-07f, 0.4589568f); // �� Unity
    [SerializeField] private Vector2 puddleColliderSize = new Vector2(0.7442487f, 0.2f);
    [SerializeField] private Vector2 puddleOffset = new Vector2(4.172325e-07f, 0.46f);
    private Sprite puddleSprite;
    private Sprite originalHeroSprite;

    private bool isPuddle = false;

    public bool IsInPuddleForm() => isPuddle;

    public void Init(Rigidbody2D rb, SpriteRenderer sr)
    {
        body = rb;
        sprite = sr;
        hero = rb.GetComponent<Hero>();
        boxCollider = hero.GetComponent<BoxCollider2D>(); // �������� BoxCollider2D
        animator = hero.GetComponentInChildren<Animator>();

        if (sprite != null)
        {
            originalHeroSprite = sprite.sprite; // Сохраняем текущий спрайт как оригинальный
        }
        else
        {
            Debug.LogError("WaterAbility: SpriteRenderer (sr) is null in Init!");
        }

        if (boxCollider == null)
        {
            Debug.LogError("WaterAbility: BoxCollider2D not found on Hero.");
        }
        if (animator == null)
        {
            Debug.LogWarning("WaterAbility: Animator not found on Hero or its children. If intended, this is fine.");
        }

        // Дополнительная проверка puddleSprite сразу после инициализации
        if (puddleSprite == null)
        {
            Debug.Log("WaterAbility (Init): puddleSprite is initially null, will be set by SetPuddleSprite().");
        }
        else
        {
            Debug.Log("WaterAbility (Init): puddleSprite was somehow pre-assigned. Name: " + puddleSprite.name);
        }
    }

    public void SetPuddleSprite(Sprite sprite)
    {
        puddleSprite = sprite;
        if (puddleSprite == null)
        {
            Debug.LogError("WaterAbility (SetPuddleSprite): Assigned puddleSprite is NULL even after setting!");
        }
        else
        {
            Debug.Log("WaterAbility (SetPuddleSprite): puddleSprite has been set. Name: " + puddleSprite.name);
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
        if (!isPuddle)
        {
            // ������� ������
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        }
    }

    public void OnLand() { }

    public void OnExit() { }

    private void EnterPuddleForm()
    {
        if (isPuddle) return;
        
        if (sprite == null) 
        {
            Debug.LogError("WaterAbility: SpriteRenderer is null in EnterPuddleForm!");
            return;
        }
        if (puddleSprite == null)
        {
            Debug.LogError("WaterAbility: puddleSprite is not assigned in the Inspector! Cannot change to puddle form.");
            return;
        }

        isPuddle = true;

        boxCollider.size = puddleColliderSize;
        boxCollider.offset = puddleOffset;
        
        sprite.enabled = true; // Убедимся, что SpriteRenderer включен
        sprite.sprite = puddleSprite; // Устанавливаем спрайт лужи

        if (animator != null)
        {
            animator.enabled = false;
        }
        else
        {
            Debug.LogWarning("WaterAbility: Animator is null, cannot disable for puddle form (may not be an issue if no animator).");
        }
        Debug.Log("Entered puddle form. Current sprite: " + (sprite.sprite != null ? sprite.sprite.name : "null"));
    }

    private void ExitPuddleForm()
    {
        if (!isPuddle) return;
        
        if (sprite == null)
        {
            Debug.LogError("WaterAbility: SpriteRenderer is null in ExitPuddleForm!");
            return;
        }

        isPuddle = false;

        boxCollider.size = normalColliderSize;
        boxCollider.offset = normalOffset;

        sprite.enabled = true; // Убедимся, что SpriteRenderer включен
        sprite.sprite = originalHeroSprite; // Восстанавливаем оригинальный спрайт

        if (animator != null)
        {
            animator.enabled = true;
        }
        else
        {
            Debug.LogWarning("WaterAbility: Animator is null, cannot re-enable (may not be an issue if no animator).");
        }
        Debug.Log("Exited puddle form. Restored sprite: " + (sprite.sprite != null ? sprite.sprite.name : "null"));
    }
}

