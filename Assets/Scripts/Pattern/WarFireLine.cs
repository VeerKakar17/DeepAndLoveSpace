using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarFireLine : MonoBehaviour
{
    Bullet bulletA;
    Bullet bulletB;
    Bullet bulletC;
    GameObject damageLinePrefab;
    bool isFiringSmall = true;
    float cooldown = 0f;
    float timerA = 0f;
    float lineTimer = 0f;
    float lineWaitTime;
    void Awake()
    {
        damageLinePrefab = Resources.Load<GameObject>("DamageLine");
        bulletA = new Bullet(
            "bullet_big",
            Color.red,
            0.14f,
            "none"
        );
        bulletB = new Bullet(
            "bullet_base",
            new Color(1f, 0.5f, 0.4f),
            0.1f,
            "none"
        );
        bulletC = new Bullet(
            "bullet_base",
            new Color(1f, 0.77f, 0.67f),
            0.1f,
            "none"
        );
        lineWaitTime = 4.5f;
        Debug.Log("Started");

        StartCoroutine(BoxMoveCoroutine());
        
    }

    private IEnumerator BoxMoveCoroutine()
    {
        yield return GameManager.Instance.player.movementBox.setScale(5.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        lineTimer += Time.deltaTime;
        if (isFiringSmall)
        {
            float dt = Time.deltaTime;
            cooldown -= dt;
            timerA += dt;

            if (timerA < 10.0f && cooldown <= 0)
            {
                cooldown = 0.2f;

                for (int i = 0; i < 4; i++)
                {
                    // this facing direction to angle
                    float angle = Mathf.Atan2(transform.up.y, transform.up.x) * Mathf.Rad2Deg;

                    // position is random position inside of string rect
                    Vector3 randomPos = new Vector3((float)Random.Range(-4, 4) + 0.5f, 5.9f, 2.5f);

                    AnimationCurve spd = new AnimationCurve();
                    float speed = Random.Range(2.5f, 3.5f);
                    spd.AddKey(0f, 6.0f);
                    spd.AddKey(1.5f, speed);
                    BulletSpawner.Instance.SpawnBullet(randomPos, angle - 90, bulletA, spd);
                    
                    AnimationCurve spd2 = new AnimationCurve();
                    float speed2 = Random.Range(1.5f, 2.5f);
                    spd2.AddKey(0f, 5.0f);
                    spd2.AddKey(1.5f, speed2);
                    Vector3 offset = new Vector3(Random.Range(-0.17f, 0.17f), 0f, 0f);
                    float colorrnd = Random.Range(0f, 1f);
                    if (colorrnd < 0.5f)
                    BulletSpawner.Instance.SpawnBullet(randomPos + offset, angle - 90, bulletB, spd2);
                    else
                    BulletSpawner.Instance.SpawnBullet(randomPos + offset, angle - 90, bulletC, spd2);

                }
            }

            if (timerA >= 13.0f)
            {
                GameManager.Instance.currentEvent.EndEvent();
            }
        }
        if (timerA <= 10.0f && lineTimer > lineWaitTime) {
            lineWaitTime = Random.Range(1.8f, 2.2f);
            lineTimer = 0f;
            float range = GameManager.Instance.player.movementBox.radius;
            float x = Random.Range(-1 * range, range);

            GameObject line = Instantiate(damageLinePrefab, new Vector3(x, -2.16f, GameManager.Instance.player.gameObject.transform.position.z + 1), Quaternion.identity, GameManager.Instance.patternContainer);
            line.transform.parent = gameObject.transform;
        }

        if (timerA >= 8f)
        {
            HeartPieceManager.Instance.ActivateNextPiece();
        }
    }
}
