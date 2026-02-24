using Unity.VisualScripting;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public float ChargeTime = 2f;
    public float ActiveTime = 1.1f;
    public float FlashTime = 0.5f;

    private float timer = 0;
    private int state = 0;
    private float lastTimestamp = 0;
    private bool yellowColor = true;

    float cooldown = 0.8f;

    int bsc = 5;

    Bullet bulletC;

    private SpriteRenderer spriteRenderer;
    private CircleCollider2D col;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = gameObject.GetComponent<CircleCollider2D>();
        col.enabled = false;
        spriteRenderer.color = new Color(1f, 0.7f, 0f, 0f);

        
        bulletC = new Bullet(
            "bullet_base",
            new Color(1.0f, 0.2f, 0.1f),
            0.1f,
            "none"
        );

    }

    void Update()
    {
        timer += Time.deltaTime;
        if (state == 0)
        {
            if (timer < ChargeTime)
            {
                lastTimestamp = timer;
                
                Color currentColor = new Color(1f, 0f, 0f, Mathf.Clamp01(timer / ChargeTime));

                spriteRenderer.color = currentColor;
            }
            if (timer >= ChargeTime)
            {
                state++;
                timer = 0;
                spriteRenderer.color = Color.red;
                col.enabled = true;
                cooldown = 0.3f;


            }
        } 
        if (state == 1)
        {

            cooldown -= Time.deltaTime;

            if (cooldown <= 0 && timer < 1.0f)
            {
                cooldown = 0.2f;


                float angleOffset = Mathf.Sin((float)Time.time * 2.0f) * 360.0f / 4.0f;
                    

                DoRadialAttack(transform.position, angleOffset, bsc);


                bsc--;
                bulletC.color = new Color(bulletC.color.r, 1.55f * bulletC.color.g, 1.65f * bulletC.color.b);
            }

            if (timer >= 1.0)
            {
                
                col.enabled = false;
                
                Color currentColor = new Color(1f, 0f, 0f, Mathf.Clamp01(1 - (timer - 1.0f) / (ActiveTime - 1.0f)));

                spriteRenderer.color = currentColor;
            }

            if (timer >= ActiveTime)
            {
                Destroy(gameObject);
            }
        }
    }

    private void DoRadialAttack(Vector3 spawnPos, float angleOffset = 0, int bcount = 4)
    {
        int BULLET_COUNT = bcount;

        for (int i = 0; i < BULLET_COUNT; i++)
        {
            BulletSpawner.Instance.SpawnBullet(spawnPos, angleOffset + 360 / BULLET_COUNT * i, bulletC, 0.9f);
        }
    }
}
