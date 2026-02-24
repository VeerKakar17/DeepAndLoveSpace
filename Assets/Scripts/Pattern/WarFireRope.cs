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
            0.1f,
            "none"
        );

        Debug.Log("Started");

        //StartCoroutine(SurroundingAttack());
        StartCoroutine(DamageSpots());
        StartCoroutine(SpawnHeart());
        StartCoroutine(BoxMoveCoroutine());
        
    }

    private IEnumerator BoxMoveCoroutine()
    {
        yield return GameManager.Instance.player.movementBox.setScale(5.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SurroundingAttack()
    {
        while (true)
        {
            float attack = Random.Range(0f, 3f);
            if (attack <= 1)
            {
                for (int i = 0; i < 10; i++)
                {
                    float angleInDegrees = (36f * i);
                    float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
                    Vector2 direction2D = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians)).normalized;
                    Vector2 spawnPos = direction2D * (GameManager.Instance.player.movementBox.radius + 0.5f);
                    
                    AnimationCurve spd = new AnimationCurve();
                    float TIME_BETWEEN_BULLETS = 0.15f;
                    spd.AddKey(0f, 0.5f);
                    spd.AddKey(TIME_BETWEEN_BULLETS*i, 0.5f);
                    spd.AddKey(1.5f+TIME_BETWEEN_BULLETS*i, 0f);
                    spd.AddKey(2.0f+TIME_BETWEEN_BULLETS*i, 2.7f);

                    BulletSpawner.Instance.SpawnBullet(GameManager.Instance.player.movementBox.gameObject.transform.position + new Vector3(spawnPos.x, spawnPos.y, -14f), angleInDegrees-90, bulletA, spd);

                }
            } 
            else if (attack <= 2)
            {
                for (int i = 0; i < 8; i++)
                {
                    // -3 to 3, 3.68
                    AnimationCurve spd = new AnimationCurve();
                    spd.AddKey(0f, 0.5f);
                    spd.AddKey(1.5f, 0.6f);
                    spd.AddKey(2.0f, 2.0f);
                    BulletSpawner.Instance.SpawnBullet(new Vector3(-3+(6f/7f)*i, 3.68f, 2f), 0f, bulletA, spd);
                }
            } 
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    // 1.5 to -4.25
                    AnimationCurve spd = new AnimationCurve();
                    spd.AddKey(0f, 0.5f);
                    spd.AddKey(1.5f, 0.6f);
                    spd.AddKey(2.0f, 2.0f);
                    BulletSpawner.Instance.SpawnBullet(new Vector3(-3, 1f - i, 2f), 90f, bulletA, spd);
                    BulletSpawner.Instance.SpawnBullet(new Vector3(3, 1.5f - i, 2f), -90f, bulletA, spd);
                }
            }
            yield return new WaitForSeconds(Random.Range(2f, 4f));
        }
    }

    private IEnumerator DamageSpots()
    {
        while (true)
        {
            for (int i = 0; i < 6; i++)
            {
                float waitTime2 = Random.Range(0.5f, 0.6f);
                yield return new WaitForSeconds(waitTime2);
                Vector2 randomInsideUnitCircle = Random.insideUnitCircle;
                randomInsideUnitCircle *= GameManager.Instance.player.movementBox.radius;
                randomInsideUnitCircle += new Vector2(GameManager.Instance.player.movementBox.gameObject.transform.position.x, GameManager.Instance.player.movementBox.gameObject.transform.position.y);

                
                SoundManager.Instance.Play("tan2", 0.2f, 0.75f, 0.1f);

                GameObject spot = Instantiate(damageSpotPrefab, new Vector3(randomInsideUnitCircle.x, randomInsideUnitCircle.y, GameManager.Instance.player.gameObject.transform.position.z+1), Quaternion.identity, GameManager.Instance.patternContainer);
                spot.transform.parent = gameObject.transform;
                yield return null;
            }
            float waitTime = Random.Range(2.6f, 2.8f);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private IEnumerator SpawnHeart()
    {
        float elapsed = 0;
        while (elapsed < 8f)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        HeartPieceManager.Instance.ActivateNextPiece();
        yield return null;
    }
}
