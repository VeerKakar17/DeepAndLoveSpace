using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class BulletMovement : MonoBehaviour
{
    public Vector3 direction = Vector3.down;
    public float speed = 5f;

    private SpriteRenderer sr;
    private CircleCollider2D col;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        // if offscreen: destroy
        Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPos.x < -0.1f || screenPos.x > 1.1 || screenPos.y < -0.1f || screenPos.y > 1.1 )
        {
            Destroy(gameObject);
        }
    }
    
    public void Initialize(
        Vector3 dir,
        float spd,
        Sprite sprite,
        Color color,
        float hitboxRadius,
        string bulletTag
    )
    {
        direction = dir.normalized;
        speed = spd;

        //sr.sprite = sprite;
        sr.color = color;

        col.radius = hitboxRadius;

        //gameObject.tag = bulletTag;
    }
}