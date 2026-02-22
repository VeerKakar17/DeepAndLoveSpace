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
            0.12f,
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

        }
    }

    private void DoRotatingCross(Vector3 spawnPos, float direction)
    {
        float rotation = Time.time * 120f * direction;

        for (int i = 0; i < 4; i++)
        {
            float angle = rotation + i * 90f;

            for (int j = -4; j <= 4; j++)
            {
                float offset = j * 10f;
                BulletSpawner.Instance.SpawnBullet(spawnPos, angle + offset, bulletA, 4f);
            }
        }
    }

    private void DoOrbiters(Vector3 spawnPos, float direction)
    {
        const int BULLET_COUNT = 8;

        float rotation = Time.time * 90f * direction;

        for (int i = 0; i < BULLET_COUNT; i++)
        {
            float angle = rotation + (360f / BULLET_COUNT) * i;

            AnimationCurve spd = new AnimationCurve();
            spd.AddKey(0f, 1f);
            spd.AddKey(1.5f, 2f);

            BulletSpawner.Instance.SpawnBullet(spawnPos, angle, bulletB, spd);
        }
    }

}
