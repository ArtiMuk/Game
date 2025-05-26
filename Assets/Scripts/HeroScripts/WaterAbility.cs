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
    [SerializeField] private Vector2 puddleOffset = new Vector2(4.172325e-07f, 0.46f);
    private Sprite puddleSprite;
    private Sprite originalHeroSprite;

    private bool isPuddle = false;

    // Свойство для проверки состояния лужи
    public bool IsInPuddleForm() => isPuddle;

    public void Init(Rigidbody2D rb, SpriteRenderer sr)
    {
        body = rb;
        sprite = sr;
        hero = rb.GetComponent<Hero>();
        boxCollider = hero.GetComponent<BoxCollider2D>();
        animator = hero.GetComponentInChildren<Animator>();

        // Сохраняем оригинальный спрайт героя
        if (sprite != null)
        {
            originalHeroSprite = sprite.sprite; // Сохраняем текущий спрайт как оригинальный
        }
    }

    // Установка спрайта для формы лужи
    public void SetPuddleSprite(Sprite sprite)
    {
        puddleSprite = sprite;
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
        // Прыжок возможен только в обычной форме
        if (!isPuddle)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        }
    }

    public void OnLand() { }

    public void OnExit() { }

    // Переход в форму лужи
    private void EnterPuddleForm()
    {
        if (isPuddle) return;
        
        if (sprite == null) 
        {
            return;
        }
        if (puddleSprite == null)
        {
            return;
        }

        isPuddle = true;

        boxCollider.size = puddleColliderSize;
        boxCollider.offset = puddleOffset;
        
        sprite.enabled = true; // SpriteRenderer включен
        sprite.sprite = puddleSprite; // Устанавливаем спрайт лужи

        if (animator != null)
        {
            animator.enabled = false;
        }
    }

    // Возврат в обычную форму
    private void ExitPuddleForm()
    {
        if (!isPuddle) return;
        
        if (sprite == null)
        {
            return;
        }

        isPuddle = false;

        boxCollider.size = normalColliderSize;
        boxCollider.offset = normalOffset;

        sprite.enabled = true; // SpriteRenderer включен
        sprite.sprite = originalHeroSprite; // Восстанавливаем оригинальный спрайт

        if (animator != null)
        {
            animator.enabled = true;
        }
    }
}

