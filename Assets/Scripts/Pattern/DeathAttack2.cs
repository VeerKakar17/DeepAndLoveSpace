using UnityEngine;
using System.Collections;

public class DeathAttack2 : MonoBehaviour
{
    private Coroutine boxCoroutine = null;
    private Coroutine radialCoroutine = null;

    public RectTransform spawnRect;

    Bullet bulletA;

    Bullet bulletB;

    void Awake()
    {
        bulletA = new Bullet(
            "bullet_moon",
            new Color(0.2f, 0.1f, 0.92f),
            0.12f,
            "no"
        );

        bulletB = new Bullet(
            "bullet_dot",
            new Color(1f, 0.85f, 1f),
            0.06f,
            "no"
        );

        radialCoroutine = StartCoroutine(RadialCoroutine());
    }

    private IEnumerator RadialCoroutine()
    {
        while (true)
        {

            for (int i = 0; i < 7; i++)
            {

                // position is random position inside of spawn rect
                Vector3 randomPos = spawnRect.position
                    + spawnRect.right * Random.Range(-spawnRect.rect.width / 2f, spawnRect.rect.width / 2f)
                    + spawnRect.up * Random.Range(-spawnRect.rect.height / 2f, spawnRect.rect.height / 2f);

                DoRotatingCross(randomPos, 1f);
                DoOrbiters(randomPos, -1f);

                yield return new WaitForSeconds(0.6f);
                
            }

            yield return new WaitForSeconds(2.6f);
            HeartPieceManager.Instance.ActivateNextPiece();
        }
    }

    private void DoRotatingCross(Vector3 spawnPos, float direction)
    {
        float rotation = Time.time * 120f * direction;

        for (int i = 0; i < 4; i++)
        {
            float angle = rotation + i * 90f;

            for (int j = -3; j <= 3; j++)
            {
                float offset = j * 12f;
                BulletSpawner.Instance.SpawnBullet(spawnPos, angle + offset, bulletA, 3f);
            }
        }
    }

    private void DoOrbiters(Vector3 spawnPos, float direction)
    {
        const int BULLET_COUNT = 12;

        float rotation = Time.time * 90f * direction;

        for (int i = 0; i < BULLET_COUNT; i++)
        {
            float angle = rotation + (360f / BULLET_COUNT) * i;

            AnimationCurve spd = new AnimationCurve();
            spd.AddKey(0f, 3.2f);
            spd.AddKey(1.5f, 0.8f);

            BulletSpawner.Instance.SpawnBullet(spawnPos, angle, bulletB, spd);
        }
    }

}
