using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public MovementBox movementBox;

    InputAction moveAction;
    InputAction focusAction;

    bool cannotmoveflag = false;

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

        if (cannotmoveflag)
        {
            moveValue = Vector2.zero;
        }

        rb.linearVelocity = moveValue;

        Vector2 dist = gameObject.transform.position - movementBox.gameObject.transform.position;
        if (dist.magnitude > movementBox.radius)
        {
            float playerZ = gameObject.transform.position.z;
            Vector2 norm = Vector2.Normalize(dist);
            gameObject.transform.position = movementBox.gameObject.transform.position + (movementBox.radius * new Vector3(norm.x, norm.y, 0f)) + new Vector3(0f, 0f, playerZ - movementBox.gameObject.transform.position.z);
        }
    }

    void clearAllBulletInBox()
    {
        // find all bullets within movement box and clear them
        Collider2D[] colliders = Physics2D.OverlapCircleAll(movementBox.gameObject.transform.position, movementBox.radius);
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Bullet"))
            {
                col.gameObject.GetComponent<BulletMovement>().ClearBullet();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("Collided");
        if (other.CompareTag("Bullet")) {
            if (invincible) return;

            invincible = true;
            timer = 0;
            GameManager.Instance.LoseLife();
            other.gameObject.GetComponent<BulletMovement>().ClearBulletImmediate();

            clearAllBulletInBox();

            StartCoroutine(DeathRoutine());
        } else if (other.CompareTag("DamageZone"))
        {
            if (invincible) return;

            invincible = true;
            timer = 0;
            GameManager.Instance.LoseLife();
            
            clearAllBulletInBox();

            StartCoroutine(DeathRoutine());
        }
    }

    private IEnumerator DeathRoutine()
    {
        // Fade color
        Color materialColor = renderer.material.color;
        materialColor.a = 1f;
        renderer.material.color = materialColor;

        cannotmoveflag = true;
        GameObject deathEffect = Instantiate(GameManager.Instance.deathEffectPrefab, transform.position, Quaternion.identity);
        Destroy(deathEffect, 1f);

        // Fade Out
        float elapsed = 0;
        const float SECONDS = 0.4f;

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
        
        cannotmoveflag = false;

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
