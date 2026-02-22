using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private MovementBox movementBox;

    InputAction moveAction;
    InputAction focusAction;
    InputAction invincibilityAction;

    const float MOVE_SPEED = 5f;
    const float FOCUS_SPEED = MOVE_SPEED / 3;

    private bool invincible = false;
    private const float INVINCIBLE_COOLDOWN_TIME = 2f;
    private float invincibleTime = 0.5f;
    private float timer = INVINCIBLE_COOLDOWN_TIME;

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        focusAction = InputSystem.actions.FindAction("Sprint");
        invincibilityAction = InputSystem.actions.FindAction("Jump");

        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Handle Movement
        bool isFocused = focusAction.IsPressed();
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        moveValue *= (isFocused? FOCUS_SPEED : MOVE_SPEED);
        rb.linearVelocity = moveValue;

        Vector2 dist = gameObject.transform.position - movementBox.gameObject.transform.position;
        if (dist.magnitude > movementBox.radius)
        {
            float playerZ = gameObject.transform.position.z;
            Vector2 norm = Vector2.Normalize(dist);
            Debug.Log("test: " + norm.x + " " + norm.y + " " + (movementBox.radius * new Vector3(norm.x, norm.y, 0f)).x + " " + (movementBox.radius * new Vector3(norm.x, norm.y, 0f)).y);
            gameObject.transform.position = movementBox.gameObject.transform.position + (movementBox.radius * new Vector3(norm.x, norm.y, 0f)) + new Vector3(0f, 0f, playerZ - movementBox.gameObject.transform.position.z);
        }

        // Handle Invincibility
        if (invincible)
        {
            timer += Time.deltaTime;
            if (timer >= invincibleTime)
            {
                invincible = false;
                timer -= invincibleTime;
                Debug.Log("Stopped Invincible");
            }
        } else if (timer < INVINCIBLE_COOLDOWN_TIME)
        {
            timer += Time.deltaTime;
            if (timer >= INVINCIBLE_COOLDOWN_TIME)
            {
                Debug.Log("Cooldown Done");
            }
        }

        if (invincibilityAction.IsPressed() && canGoInvincible())
        {
            invincible = true;
            timer = 0;
            Debug.Log("Went Invincible");
        }
    }

    private bool canGoInvincible()
    {
        return !invincible && timer >= INVINCIBLE_COOLDOWN_TIME;
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("Collided");
        if (other.CompareTag("Bullet")) {
            if (invincible) return;

            invincible = true;
            timer = 0;
            GameManager.Instance.LoseLife();
            Destroy(other.gameObject);

            gameObject.transform.position = movementBox.gameObject.transform.position;
        }
    }
}
