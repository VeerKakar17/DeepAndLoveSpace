using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class MovementBox : MonoBehaviour
{
    InputAction invincibilityAction;
    bool canPress = true;
    public float radius = 1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.transform.localScale = new Vector3(radius * 2, radius * 2, 1f);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator updateScale(float scale, float seconds)
    {
        yield return updateScale(scale, scale, seconds);
    }

    public IEnumerator updateScale(float scaleX, float scaleY, float seconds)
    {
        Vector3 start = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, 1f);
        Vector3 end = new Vector3(gameObject.transform.localScale.x * scaleX, gameObject.transform.localScale.y * scaleY, 1f);
        float elapsedTime = 0;

        while (elapsedTime < seconds)
        {
            float t = elapsedTime / seconds;

            gameObject.transform.localScale = Vector3.Lerp(start, end, t);
            radius = gameObject.transform.localScale.x / 2f;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        gameObject.transform.localScale = end;
    }

    public void updateScaleImmediate(float scaleX, float scaleY)
    {
        Vector3 end = new Vector3(gameObject.transform.localScale.x * scaleX, gameObject.transform.localScale.y * scaleY, 1f);
        gameObject.transform.localScale = end;
    }

    public IEnumerator updateRotation(float degrees, float seconds)
    {
        Quaternion start = gameObject.transform.localRotation;
        gameObject.transform.Rotate(0, 0, degrees);
        Quaternion end = gameObject.transform.localRotation;
        gameObject.transform.rotation = start;
        float elapsedTime = 0;

        while (elapsedTime < seconds)
        {
            float t = elapsedTime / seconds;

            gameObject.transform.localRotation = Quaternion.Slerp(start, end, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        gameObject.transform.localRotation = end;
    }

    public void updateRotationImmediate(float degrees)
    {
        gameObject.transform.Rotate(0, 0, degrees);
    }

    public IEnumerator updatePosition(float x, float y, float seconds)
    {
        Vector3 start = gameObject.transform.position;
        Vector3 end = gameObject.transform.position + new Vector3(x, y, 0f);
        float elapsedTime = 0;

        while (elapsedTime < seconds)
        {
            float t = elapsedTime / seconds;

            gameObject.transform.position = Vector3.Lerp(start, end, t);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        gameObject.transform.position = end;
    }

    public void updatePositionImmediate(float x, float y)
    {
        Vector3 end = gameObject.transform.position + new Vector3(x, y, 0f);
        gameObject.transform.position = end;
    }

    public void Transform(Vector3 position, float scale, float time)
    {
        StartCoroutine(updatePosition(position.x, position.y, time));
        StartCoroutine(updateScale(scale, scale, time));
    }

    public void TransformImmediate(Vector3 position, float scale)
    {
        updatePositionImmediate(position.x, position.y);
        updateScaleImmediate(scale, scale);
    }
}
