using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public Vector3 direction = Vector3.down;
    public float speed = 5f;

    float createTime;
    
    BulletSpawner pool;

    AnimationCurve speedCurve;

    private SpriteRenderer sr;
    private CircleCollider2D col;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<CircleCollider2D>();
    }

    void Update()
    {

        // update speed
        float t = Time.time - createTime;
        if (t <= speedCurve.keys[speedCurve.length - 1].time) speed = speedCurve.Evaluate(t);

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
        AnimationCurve spd,
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

        speedCurve = spd;
        speed = speedCurve.Evaluate(0);

        sr.sprite = sprite;
        sr.color = color;

        col.radius = hitboxRadius;

        createTime = Time.time;

        //gameObject.tag = bulletTag;
    }
}