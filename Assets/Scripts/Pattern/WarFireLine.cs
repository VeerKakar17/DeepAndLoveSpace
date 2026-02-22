using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarFireLine : MonoBehaviour
{
    Bullet bulletA;
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
            0.1f,
            "none"
        );
        lineWaitTime = Random.Range(3f, 6f);
        Debug.Log("Started");
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

                for (int i = 0; i < 3; i++)
                {
                    // this facing direction to angle
                    float angle = Mathf.Atan2(transform.up.y, transform.up.x) * Mathf.Rad2Deg;

                    // position is random position inside of string rect
                    Vector3 randomPos = new Vector3(Random.Range(-3, 3), 3.6f, 2.5f);

                    float speed = Random.Range(2f, 3f);
                    BulletSpawner.Instance.SpawnBullet(randomPos, angle - 90, bulletA, speed);

                }
            }

            if (timerA >= 13.0f)
            {
                GameManager.Instance.currentEvent.EndEvent();
            }
        }
        if (timerA <= 10.0f && lineTimer > lineWaitTime) {
            lineWaitTime = Random.Range(2f, 5f);
            lineTimer = 0f;
            float range = GameManager.Instance.player.movementBox.radius;
            float x = Random.Range(-1 * range, range);

            GameObject line = Instantiate(damageLinePrefab, new Vector3(x, -2.16f, GameManager.Instance.player.gameObject.transform.position.z + 1), Quaternion.identity);
            line.transform.parent = gameObject.transform;
        }

        if (timerA >= 8f)
        {
            HeartPieceManager.Instance.ActivateNextPiece();
        }
    }
}
