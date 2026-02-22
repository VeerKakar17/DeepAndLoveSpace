using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class BulletMovement : MonoBehaviour
{
    public Vector3 direction = Vector3.down;
    public float speed = 5f;
    
    BulletSpawner pool;

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
            ClearBullet();
        }
    }

    public void ClearBullet()
    {
        pool.PoolReturn(gameObject);
    }
    
    public void Initialize(
        BulletSpawner p,
        Vector3 pos,
        float angle,
        float spd,
        Sprite sprite,
        Color color,
        float hitboxRadius,
        string bulletTag
    )
    {
        pool = p;

        transform.position = pos;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        
        angle -= 90.0f;
        
        direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);

        speed = spd;

        sr.sprite = sprite;
        sr.color = color;

        col.radius = hitboxRadius;

        //gameObject.tag = bulletTag;
    }
}