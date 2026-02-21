using UnityEngine;

public class SnapToTarget : MonoBehaviour
{
    public Transform snapTarget;
    public float snapSpeed = 10f;
    public bool snapInstantly = false;

    private bool snapping = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Something touched me: " + other.name);
        if (other.CompareTag("Player"))
        {
            StartSnapping();
        }
    }

    void StartSnapping()
    {
        snapping = true;

        if (snapInstantly)
        {
            transform.position = snapTarget.position;
            transform.rotation = snapTarget.rotation;
            snapping = false;
        }
    }

    void Update()
    {
        if (!snapping) return;

        transform.position = Vector3.Lerp(
            transform.position,
            snapTarget.position,
            snapSpeed * Time.deltaTime
        );

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            snapTarget.rotation,
            snapSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, snapTarget.position) < 0.01f)
        {
            transform.position = snapTarget.position;
            transform.rotation = snapTarget.rotation;
            snapping = false;
        }
    }
}