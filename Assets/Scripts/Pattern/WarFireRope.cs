using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarFireRope : MonoBehaviour
{
    Bullet bulletA;
    GameObject damageSpotPrefab;
    void Awake()
    {
        damageSpotPrefab = Resources.Load<GameObject>("DamageSpot");
        bulletA = new Bullet(
            "bullet_big",
            Color.red,
            0.3f,
            "none"
        );

        Debug.Log("Started");

        StartCoroutine(SurroundingAttack());
        StartCoroutine(DamageSpots());
        StartCoroutine(SpawnHeart());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SurroundingAttack()
    {
        int attack = 0;
        if (attack <= 1)
        {
            List<BulletMovement> bullets = new List<BulletMovement>();
            for (int i = 0; i < 10; i++)
            {
                float angleInDegrees = (36f * i);
                float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
                Vector2 direction2D = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians)).normalized;
                Vector2 spawnPos = direction2D * (GameManager.Instance.player.movementBox.radius + 0.5f);
                bullets.Add(BulletSpawner.Instance.SpawnBullet(GameManager.Instance.player.movementBox.gameObject.transform.position + new Vector3(spawnPos.x, spawnPos.y, -14f), angleInDegrees, bulletA, 0f));
            }

            yield return new WaitForSeconds(1f);

            const float TIME_BETWEEN_SHOTS = 0.3f;
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(TIME_BETWEEN_SHOTS);

                BulletMovement next = bullets[0];
                bullets.RemoveAt(0);
                BulletSpawner.Instance.SpawnBullet(next.gameObject.transform.position, (36f * i) - 90, bulletA, 5f);
                Destroy(next.gameObject);
            }
        } 
        else if (attack <= 2)
        {
            List<BulletMovement> bullets = new List<BulletMovement>();
            for (int i = 0; i < 6; i++)
            {
                float angleInDegrees = (36f * i);
                float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
                Vector2 direction2D = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians)).normalized;
                Vector2 spawnPos = direction2D * (GameManager.Instance.player.movementBox.radius + 0.5f);
                bullets.Add(BulletSpawner.Instance.SpawnBullet(GameManager.Instance.player.movementBox.gameObject.transform.position + new Vector3(spawnPos.x, spawnPos.y, -14f), angleInDegrees, bulletA, 0f));
            }

            yield return new WaitForSeconds(1f);

            const float TIME_BETWEEN_SHOTS = 0.3f;
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(TIME_BETWEEN_SHOTS);

                BulletMovement next = bullets[0];
                bullets.RemoveAt(0);
                BulletSpawner.Instance.SpawnBullet(next.gameObject.transform.position, (36f * i) - 90, bulletA, 5f);
                Destroy(next.gameObject);
            }
        } 
        else
        {
            List<BulletMovement> bullets = new List<BulletMovement>();
            for (int i = 0; i < 10; i++)
            {
                float angleInDegrees = (36f * i);
                float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
                Vector2 direction2D = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians)).normalized;
                Vector2 spawnPos = direction2D * (GameManager.Instance.player.movementBox.radius + 0.5f);
                bullets.Add(BulletSpawner.Instance.SpawnBullet(GameManager.Instance.player.movementBox.gameObject.transform.position + new Vector3(spawnPos.x, spawnPos.y, -14f), angleInDegrees, bulletA, 0f));
            }

            yield return new WaitForSeconds(1f);

            const float TIME_BETWEEN_SHOTS = 0.3f;
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(TIME_BETWEEN_SHOTS);

                BulletMovement next = bullets[0];
                bullets.RemoveAt(0);
                BulletSpawner.Instance.SpawnBullet(next.gameObject.transform.position, (36f * i) - 90, bulletA, 5f);
                Destroy(next.gameObject);
            }
        }
    }

    private IEnumerator DamageSpots()
    {
        while (true)
        {
            float waitTime = Random.Range(1f, 5f);
            yield return new WaitForSeconds(waitTime);
            Vector2 randomInsideUnitCircle = Random.insideUnitCircle;
            randomInsideUnitCircle *= GameManager.Instance.player.movementBox.radius;
            randomInsideUnitCircle += new Vector2(GameManager.Instance.player.movementBox.gameObject.transform.position.x, GameManager.Instance.player.movementBox.gameObject.transform.position.y);

            GameObject spot = Instantiate(damageSpotPrefab, new Vector3(randomInsideUnitCircle.x, randomInsideUnitCircle.y, GameManager.Instance.player.gameObject.transform.position.z+1), Quaternion.identity);
            spot.transform.parent = gameObject.transform;
            yield return null;
        }
    }

    private IEnumerator SpawnHeart()
    {
        float elapsed = 0;
        while (true)
        {
            elapsed += Time.deltaTime;
            if (elapsed > 7f)
            {
                HeartPieceManager.Instance.ActivateNextPiece();
            }
            yield return null;
        }
    }
}
