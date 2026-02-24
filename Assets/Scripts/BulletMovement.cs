using UnityEngine;
using System.Collections;

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

    bool isClearing = false;

    string BulletTag;

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

        if (BulletTag == "famine_a_spawner")
        {
            UpdateSpecialFamineASpawner();
        }

        // if offscreen: destroy
        Vector3 wPos = transform.position;
        if (t > 0.3f && (wPos.x < -5.0f || wPos.x > 5.0f || wPos.y < -6.0f || wPos.y > 6.0f))
        {
            ClearBulletImmediate();
        }
    }

    bool IsInScreen()
    {
        Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);
        return screenPos.x >= 0 && screenPos.x <= 1 && screenPos.y >= 0 && screenPos.y <= 1;
    }

    
    void UpdateSpecialFamineASpawner()
    {

        if (isClearing) return;
        
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

                    float waittime = 2.4f - 0.06f * counter;
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

    Coroutine removeCoroutine;
    public void ClearBullet()
    {
        if (!isClearing)
            removeCoroutine = StartCoroutine(FadeAndClear());
    }

    public void ClearBulletImmediate()
    {
        if (isClearing)
        {
            StopCoroutine(removeCoroutine);
        }
        pool.PoolReturn(gameObject);
        
    }

    private IEnumerator FadeAndClear()
    {
        isClearing = true;

        float duration = 0.5f;
        float t = 0f;

        col.enabled = false;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color startColor = sr.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, 0f, t / duration);
            sr.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

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

        this.BulletTag = bulletTag;

        col.enabled = true;
        isClearing = false;

        this.cooldown = 0f;

        if (BulletTag == "famine_a_spawner")
        {
            cooldown = 0.9f;
        }
    }
}