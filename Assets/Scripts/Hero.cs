using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jumpForce = 13f;

    private bool isOnGround = false;
    private Rigidbody2D body;
    private SpriteRenderer sprite;

    private IAbility currentAbility;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetButton("Horizontal"))
            Run();

        if (isOnGround && Input.GetButtonDown("Jump"))
            Jump();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (currentAbility == null)
            {
                var wind = gameObject.AddComponent<WindAbility>();
                wind.Init(body, sprite);
                currentAbility = wind;
                Debug.Log("Способность ветра активирована!");
            }
        }

        currentAbility?.OnUpdate();
    }

    private void FixedUpdate()
    {
        CheckIsOnGround();
        currentAbility?.OnFixedUpdate();
    }

    private void Jump()
    {
        if (currentAbility != null)
        {
            currentAbility.OnJump();
        }
        else
        {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Run()
    {
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        sprite.flipX = direction.x < 0.0f;
    }

    private void CheckIsOnGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        bool wasGrounded = isOnGround;
        isOnGround = colliders.Length > 1;

        if (!wasGrounded && isOnGround)
        {
            currentAbility?.OnLand(); // Сброс двойного прыжка, если есть
        }
    }
}
