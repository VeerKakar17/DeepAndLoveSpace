using UnityEngine;
using System.Collections;

public class DeathAttack1 : MonoBehaviour
{
    private Coroutine boxCoroutine = null;
    private Coroutine radialCoroutine = null;

    public RectTransform spawnRect;

    Bullet bulletA;

    void Awake()
    {
        bulletA = new Bullet(
            "bullet_drop",
            new Color(0.1f, 0.1f, 0.3f),
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

                DoSpiralAttack(randomPos);

                yield return new WaitForSeconds(0.6f);
            }

            yield return new WaitForSeconds(2.6f);
            HeartPieceManager.Instance.ActivateNextPiece();
        }
    }

    // Spiral attack, basic.
    private float spiralAngle = 0f;

    // Coroutine for spiral.
    private IEnumerator SpiralCoroutine(Vector3 spawnPos)
    {
        while (true)
        {
            // For speed of spiral.
            spiralAngle += 8f;
            BulletSpawner.Instance.SpawnBullet(spawnPos, spiralAngle, bulletA, 3f);
            yield return new WaitForSeconds(0.03f);
        }
    }

    private IEnumerator SpiralRadialCoroutine(Vector3 spawnPos)
    {
        const int BULLET_COUNT = 20;
        const float ROTATION_SPEED = 30f;
        const float FIRE_RATE = 0.1f;
        const float DURATION = 5f;

        float elapsed = 0f;
        float rotationOffset = 0f;

        while (elapsed < DURATION)
        {
            rotationOffset += ROTATION_SPEED * FIRE_RATE;

            for (int i = 0; i < BULLET_COUNT; i++)
            {
                float angle = rotationOffset + (360f / BULLET_COUNT) * i;

                AnimationCurve spd = new AnimationCurve();
                spd.AddKey(0f, 1f);
                spd.AddKey(0.5f, 3f);

                BulletSpawner.Instance.SpawnBullet(spawnPos, angle, bulletA, spd);
            }

            yield return new WaitForSeconds(FIRE_RATE);
            elapsed += FIRE_RATE;
        }
    }

    private Coroutine spiralRoutine;
    private void DoSpiralAttack(Vector3 spawnPos)
    {
        if (spiralRoutine != null)
            StopCoroutine(spiralRoutine);

        spiralRoutine = StartCoroutine(SpiralRadialCoroutine(spawnPos));
    }

}
