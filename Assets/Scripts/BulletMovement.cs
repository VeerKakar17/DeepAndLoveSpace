using UnityEngine;

public class Util
{
    public static void MakeCurveLinear(AnimationCurve curve)
    {
        for (int i = 0; i < curve.length; i++)
        {
            Keyframe key = curve[i];

            if (i > 0)
            {
                float slope = (curve[i].value - curve[i - 1].value) /
                            (curve[i].time - curve[i - 1].time);
                key.inTangent = slope;
            }

            if (i < curve.length - 1)
            {
                float slope = (curve[i + 1].value - curve[i].value) /
                            (curve[i + 1].time - curve[i].time);
                key.outTangent = slope;
            }

            curve.MoveKey(i, key);
        }
    }
}

public class BulletMovement : MonoBehaviour
{
    public Vector3 direction = Vector3.down;
    public float speed = 5f;

    float createTime;
    
    BulletSpawner pool;

    AnimationCurve speedCurve;

    string tag;

    float cooldown = 0f;
    int counter = 0;

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
        if (t <= speedCurve.keys[speedCurve.length - 1].time) speed = Mathf.Max(0f, speedCurve.Evaluate(t));
        else speed = speedCurve.keys[speedCurve.length - 1].value;
        

        transform.position += direction * speed * Time.deltaTime;

        if (tag == "famine_a_spawner")
        {
            UpdateSpecialFamineASpawner();
        }

        // if offscreen: destroy
        Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPos.x < -0.2f || screenPos.x > 1.2 || screenPos.y < -0.2f || screenPos.y > 1.2 )
        {
            ClearBullet();
        }
    }

    bool IsInScreen()
    {
        Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);
        return screenPos.x >= 0 && screenPos.x <= 1 && screenPos.y >= 0 && screenPos.y <= 1;
    }

    
    void UpdateSpecialFamineASpawner()
    {
        
        if (IsInScreen())
        {
            float dt = Time.deltaTime;
            cooldown -= dt;
            
            if (cooldown <= 0)
            {
                cooldown = 0.1f;

                for (int i = 0; i < 1; i++)
                {
                    
                    counter++;

                    Vector3 position = transform.position;

                    float waittime = 3.0f - 0.1f * counter;
                    if (waittime < 0.5f) waittime = 0.5f;

                    AnimationCurve spd = new AnimationCurve();
                    spd.AddKey(0f, 0f);
                    spd.AddKey(waittime, 0.2f);
                    spd.AddKey(waittime + 0.1f, 0.8f);

                    Bullet spawnBullet = new Bullet(
                        "bullet_base",
                        new Color(0.4f, 0.7f, 0.7f),
                        0.1f,
                        "none"
                    );

                    BulletSpawner.Instance.SpawnBullet(position, counter * 28f, spawnBullet, spd);
                }
            }
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

        this.tag = bulletTag;

        this.cooldown = 0f;

        if (tag == "famine_a_spawner")
        {
            cooldown = 1.5f;
        }
    }
}