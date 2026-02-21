using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D ))]
public class Bullet : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;

    [Header("Visual")]
    public Color bulletColor = Color.white;

    [Header("Hitbox")]
    public float hitboxRadius = 0.2f;

    private SpriteRenderer sr;
    private CircleCollider2D col;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<CircleCollider2D>();
    }

    void Start()
    {
        UpdateSettings();
    }

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
    }

    public void UpdateSettings()
    {
        sr.color = bulletColor;
        col.radius = hitboxRadius;
    }
}