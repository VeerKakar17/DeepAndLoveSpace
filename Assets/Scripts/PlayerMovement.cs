using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    InputAction moveAction;
    InputAction focusAction;
    InputAction invincibilityAction;

    const float MOVE_SPEED = 5f;
    const float FOCUS_SPEED = MOVE_SPEED / 3;

    private bool invincible = false;
    private const float INVINCIBLE_COOLDOWN_TIME = 2f;
    private float invincibleTime = 0.5f;
    private float timer = INVINCIBLE_COOLDOWN_TIME;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        focusAction = InputSystem.actions.FindAction("FocusMove");
        invincibilityAction = InputSystem.actions.FindAction("Invincible");
    }

    // Update is called once per frame
    void Update()
    {
        // Handle Movement
        bool isFocused = focusAction.IsPressed();
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        moveValue *= Time.deltaTime * (isFocused? FOCUS_SPEED : MOVE_SPEED);
        gameObject.transform.position += new Vector3(moveValue.x, moveValue.y, 0);
    
        // Handle Invincibility
        if (invincible)
        {
            timer += Time.deltaTime;
            if (timer >= invincibleTime)
            {
                invincible = false;
                timer -= invincibleTime;
            }
        } else if (timer < INVINCIBLE_COOLDOWN_TIME)
        {
            timer += Time.deltaTime;
        }

        if (invincibilityAction.IsPressed() && canGoInvincible())
        {
            invincible = true;
            timer = 0;
        }
    }

    private bool canGoInvincible()
    {
        return !invincible && timer >= INVINCIBLE_COOLDOWN_TIME;
    }
}
