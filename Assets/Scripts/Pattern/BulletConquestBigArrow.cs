using UnityEngine;

public class BulletConquestBigArrow : MonoBehaviour
{
    Bullet leafBullet;

    bool isMoving = false;
    bool isSpawning = false;

    float cooldown = 0.0f;

    float speed = 3f;
    
    Vector3 direction;

    void Awake()
    {
        leafBullet = new Bullet(
            "bullet_leaf",
            new Color(1f, 0.9f, 0.31f),
            0.1f,
            "none"
        );

        // direction = forward of the arrow
        direction = -transform.up;

        StateA();
    }

    public void StateA()
    {
        isMoving = true;
        isSpawning = true;
    }

    void Update()
    {
        
        if (isMoving)
        {
            
            transform.position += direction * speed * Time.deltaTime;

            // if offscreen: destroy
            Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);
            if (screenPos.x < -0.1f || screenPos.x > 1.1 || screenPos.y < -0.1f || screenPos.y > 1.1 )
            {
                Destroy(gameObject);
            }

        }

        if (isSpawning)
        {
            float dt = Time.deltaTime;
            cooldown -= dt;
            
            if (cooldown <= 0)
            {
                cooldown = 0.4f;

                for (int i = 0; i < 4; i++)
                {
                    // spawn small leafs at the arrow, aimed to an angle centered at back of the arrow
                    Vector3 position = transform.position - transform.up * 0.5f;
                    float angle = Mathf.Atan2(-transform.up.y, -transform.up.x) * Mathf.Rad2Deg;
                    
                    if (i % 2 == 0)
                    {
                        angle += 30f;
                    }
                    else
                    {
                        angle -= 30f;
                    }

                    AnimationCurve speed = new AnimationCurve();

                    if (i >= 2)
                    {
                        speed.AddKey(0f, 0f);
                        speed.AddKey(0.3f, 0f);
                        speed.AddKey(0.4f, 1.2f);
                    }
                    else
                    {
                        speed.AddKey(0f, 1.2f);
                    }

                    BulletSpawner.Instance.SpawnBullet(position, angle - 90, leafBullet, speed);
                }
            }
        }
    }
}
