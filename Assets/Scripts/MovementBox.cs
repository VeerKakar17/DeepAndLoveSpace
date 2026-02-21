using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementBox : MonoBehaviour
{
    InputAction invincibilityAction;
    bool canPress = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.transform.localScale = new Vector3(3f, 3f, 1f);
        invincibilityAction = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void Update()
    {
        if (canPress && invincibilityAction.IsPressed())
        {
            canPress = false;
            updateRotation(30f);
        } else if (!invincibilityAction.IsPressed())
        {
            canPress = true;
        }
    }

    public void updateScale(float scale)
    {
        updateScale(scale, scale);
    }

    public void updateScale(float scaleX, float scaleY)
    {
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * scaleX, gameObject.transform.localScale.y * scaleY, 1f);
    }

    public void updateRotation(float degrees)
    {
        gameObject.transform.Rotate(0, 0, degrees);
    }

    public void updatePosition(float x, float y)
    {
        gameObject.transform.position += new Vector3(x, y, 1f);
    }
}
