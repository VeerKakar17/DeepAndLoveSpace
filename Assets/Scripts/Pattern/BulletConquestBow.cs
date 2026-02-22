using UnityEngine;

public class BulletConquestBow : MonoBehaviour
{

    public RectTransform stringTransform;

    public GameObject bigArrowPrefab;

    Bullet arrowBullet;

    bool isAiming = false;
    bool isFiringSmall = false;

    float cooldown = 0.0f;

    void Awake()
    {
        arrowBullet = new Bullet(
            3f,
            "bullet_arrow",
            Color.yellow,
            0.1f,
            "none"
        );

        StateA();
    }

    public void StateA()
    {
        isAiming = true;
        isFiringSmall = true;
    }

    void SpawnBigArrow()
    {
        
    }

    void Update()
    {
        if (isAiming)
        {

            float timescale = 1f;
            float offset = 1f;

            // facing player
            Vector3 PlayerPosition = GameManager.Instance.player.transform.position;
            // slightly offset  player pos based on time
            PlayerPosition += new Vector3(Mathf.Sin(Time.time * timescale) * offset, 0);
            Vector3 direction = (PlayerPosition - transform.position) / 2f;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle += 90.0f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (isFiringSmall)
        {
            float dt = Time.deltaTime;
            cooldown -= dt;
            
            if (cooldown <= 0)
            {
                cooldown = 0.2f;
                
                for (int i = 0; i < 4; i++)
                {

                    // this facing direction to angle
                    float angle = Mathf.Atan2(transform.up.y, transform.up.x) * Mathf.Rad2Deg;

                    // position is random position inside of string rect
                    Vector3 randomPos = stringTransform.position
                        + stringTransform.right * Random.Range(-stringTransform.rect.width / 2f, stringTransform.rect.width / 2f)
                        + stringTransform.up * Random.Range(-stringTransform.rect.height / 2f, stringTransform.rect.height / 2f);
                    
                    float speed = Random.Range(2f, 3f);
                    arrowBullet.speed = speed;

                    BulletSpawner.Instance.SpawnBullet(randomPos, angle - 90, arrowBullet);
                        
                }

            }
        }
    }
}
