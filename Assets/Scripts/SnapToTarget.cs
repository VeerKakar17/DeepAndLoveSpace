using UnityEngine;

public class SnapToTarget : MonoBehaviour
{
    public Transform snapTarget;
    public float snapSpeed = 10f;
    public bool snapInstantly = false;

    private bool snapping = false;

    public bool touched = false;

    private HeartPiece heartPiece;
    private HeartPieceManager manager;

    void Start()
    {
        heartPiece = GetComponent<HeartPiece>();
        manager = FindObjectOfType<HeartPieceManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Start snapping to: " + other.name);
            StartSnapping();
        }
    }

    void StartSnapping()
    {
        // remove dark cycle script
        HeartDarkCycle darkCycle = GetComponent<HeartDarkCycle>();
        if (darkCycle != null)
        {
            Destroy(darkCycle);
        }
        // remove circle collider 2d
        CircleCollider2D col = GetComponent<CircleCollider2D>();
        if (col != null)
        {
            Destroy(col);
        }
        // reparent?
        // transform.SetParent(snapTarget, true);
        // stop attack code here
        GameManager.Instance.currentEvent.EndEvent();

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

            if (heartPiece != null)
                heartPiece.OnSnapped();
            if (manager != null)
            {
                Debug.Log("Activating next piece");
                manager.ActivateNextPiece();
            }
        }
    }
}