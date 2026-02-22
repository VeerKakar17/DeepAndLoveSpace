using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public MovementBox movementBox;

    InputAction moveAction;
    InputAction focusAction;
    InputAction invincibilityAction;

    const float MOVE_SPEED = 5f;
    const float FOCUS_SPEED = MOVE_SPEED / 3;

    private bool invincible = false;
    private const float INVINCIBLE_COOLDOWN_TIME = 2f;
    private float invincibleTime = 0.5f;
    private float timer = INVINCIBLE_COOLDOWN_TIME;
    private const float INVINCIBLE_FADED_ALPHA = 0.4f;

    private Rigidbody2D rb;
    Renderer renderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        focusAction = InputSystem.actions.FindAction("Sprint");
        invincibilityAction = InputSystem.actions.FindAction("Jump");

        rb = gameObject.GetComponent<Rigidbody2D>();
        renderer = GetComponent<Renderer>();
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
                
                Color materialColor = renderer.material.color;
                materialColor.a = 1f;
                renderer.material.color = materialColor;

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

            Color materialColor = renderer.material.color;
            materialColor.a = INVINCIBLE_FADED_ALPHA;
            renderer.material.color = materialColor;

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
            other.gameObject.GetComponent<BulletMovement>().ClearBullet();

            StartCoroutine(DeathRoutine());
        }
    }

    private IEnumerator DeathRoutine()
    {
        // Fade color
        Color materialColor = renderer.material.color;
        materialColor.a = 1f;
        renderer.material.color = materialColor;

        // Fade Out
        float elapsed = 0;
        const float SECONDS = 0.1f;

        while (elapsed < SECONDS)
        {
            float t = elapsed / SECONDS;

            materialColor.a = Mathf.Lerp(1f, 0f, t);
            renderer.material.color = materialColor;

            elapsed += Time.deltaTime;
            yield return null;
        }
        materialColor.a = 0f;
        renderer.material.color = materialColor;

        // Goto center
        gameObject.transform.position = new Vector3(movementBox.gameObject.transform.position.x, movementBox.gameObject.transform.position.y, gameObject.transform.position.z);
        timer = 0;

        // Fade in
        elapsed = 0;

        while (elapsed < SECONDS)
        {
            float t = elapsed / SECONDS;

            materialColor.a = Mathf.Lerp(0f, 1f, t);
            renderer.material.color = materialColor;

            elapsed += Time.deltaTime;
            yield return null;
        }
        materialColor.a = 1f;
        renderer.material.color = materialColor;

        
        // Flash invincibility
        elapsed = 0;
        bool isFaded = false;
        for (int i = 0; i < 2; i++)
        {
            isFaded = !isFaded;

            materialColor.a = isFaded? INVINCIBLE_FADED_ALPHA : 1f;
            renderer.material.color = materialColor;

            yield return new WaitForSeconds(0.2f);
        }

        materialColor.a = 1f;
        renderer.material.color = materialColor;
        invincible = false;
        timer = 0;
    }

}
