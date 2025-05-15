using UnityEngine;

public class WaterAbility : MonoBehaviour, IAbility
{
    private Rigidbody2D body;
    private SpriteRenderer sprite;
    private Hero hero;
    private BoxCollider2D boxCollider; // �������� CapsuleCollider2D �� BoxCollider2D

    [SerializeField] private float jumpForce = 13f;
    [SerializeField] private Vector2 normalColliderSize = new Vector2(0.7442487f, 0.9808896f); // �� Unity
    [SerializeField] private Vector2 normalOffset = new Vector2(4.172325e-07f, 0.4589568f); // �� Unity
    [SerializeField] private Vector2 puddleColliderSize = new Vector2(0.7442487f, 0.2f);
    [SerializeField] private Vector2 puddleOffset = new Vector2(4.172325e-07f, 0.068512f);
    [SerializeField] private Sprite puddleSprite;
    [SerializeField] private Sprite normalSprite;

    private bool isPuddle = false;

    public void Init(Rigidbody2D rb, SpriteRenderer sr)
    {
        body = rb;
        sprite = sr;
        hero = rb.GetComponent<Hero>();
        boxCollider = hero.GetComponent<BoxCollider2D>(); // �������� BoxCollider2D

        if (boxCollider == null)
        {
            Debug.LogError("WaterAbility: BoxCollider2D not found on Hero.");
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
        isPuddle = true;

        // �������� ������ � �������� ��� BoxCollider2D
        boxCollider.size = puddleColliderSize;
        boxCollider.offset = puddleOffset;
        sprite.sprite = puddleSprite;

        Debug.Log("Entered puddle form.");
    }

    private void ExitPuddleForm()
    {
        if (!isPuddle) return;
        isPuddle = false;

        // ���������� ������� ���������� ������� � ��������
        boxCollider.size = normalColliderSize;
        boxCollider.offset = normalOffset;
        sprite.sprite = normalSprite;

        Debug.Log("Exited puddle form.");
    }
}

