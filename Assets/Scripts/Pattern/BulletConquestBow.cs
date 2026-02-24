using UnityEngine;
using UnityEngine.AI;

public class BulletConquestBow : MonoBehaviour
{

    public RectTransform stringTransform;

    public GameObject bigArrowPrefab;

    Bullet arrowBullet;

    bool isAiming = false;
    bool isFiringSmall = false;
    bool isFiringBig = false;

    float cooldown = 0.0f;

    float timerA = 0.0f;

    void Awake()
    {
        arrowBullet = new Bullet(
            "bullet_arrow",
            new Color(1f, 0.9f, 0.31f),
            0.1f,
            "none"
        );

        StartPattern();
    }

    public void StartPattern()
    {
        BossControl boss = GameManager.Instance.bossControl;
        if (boss is BossControlConquest bc)
        {
            bc.HideBow();
        }

        StateA();
    }

    public void StateA()
    {
        isAiming = true;
        isFiringSmall = true;
        isFiringBig = false;

        timerA = 0.0f;
    }

    public void StateB()
    {
        isAiming = true;
        isFiringSmall = false;
        isFiringBig = true;

        timerA = 0.0f;
        cooldown = 0.0f;
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
            timerA += dt;
            
            if (cooldown <= 0)
            {
                cooldown = 0.2f;
                
                for (int i = 0; i < 2; i++)
                {

                    // this facing direction to angle
                    float angle = Mathf.Atan2(transform.up.y, transform.up.x) * Mathf.Rad2Deg;

                    // position is random position inside of string rect
                    Vector3 randomPos = stringTransform.position
                        + stringTransform.right * Random.Range(-stringTransform.rect.width / 2f, stringTransform.rect.width / 2f)
                        + stringTransform.up * Random.Range(-stringTransform.rect.height / 2f, stringTransform.rect.height / 2f);
                    
                    float speed = Random.Range(2f, 3f);
                    BulletSpawner.Instance.SpawnBullet(randomPos, angle - 90, arrowBullet, speed);
                        
                }
            }

            if (timerA >= 5.0f)
            {
                StateB();
            }
        }

        if (isFiringBig)
        {
            float dt = Time.deltaTime;
            cooldown -= dt;
            timerA += dt;
            
            if (cooldown <= 0)
            {
                cooldown = 2.0f;
                
                    // this facing direction to angle
                    float angle = Mathf.Atan2(transform.up.y, transform.up.x) * Mathf.Rad2Deg;

                    // position is center of self transform
                    Vector3 position = new Vector3(transform.position.x, transform.position.y, -8.5f);

                    Instantiate(bigArrowPrefab, position, Quaternion.Euler(0, 0, angle - 90), GameManager.Instance.patternContainer);

                    if (timerA >= 4.0f)
                    {
                        HeartPieceManager.Instance.ActivateNextPiece();
                    }
            }

            if (timerA >= 12.0f)
            {
                cooldown = 999f;

                if (timerA >= 15.0f)
                {
                    EndPattern();
                }
            }
        }
    }

    void EndPattern()
    {
        BossControl boss = GameManager.Instance.bossControl;
        if (boss is BossControlConquest bc)
        {
            bc.ShowBow();
        }

        Destroy(gameObject);
        GameManager.Instance.currentEvent.EndEvent();
    }
}
