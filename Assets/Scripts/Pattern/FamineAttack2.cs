using UnityEngine;
using System.Collections;

public class FamineAttack2 : MonoBehaviour
{
    private Coroutine boxCoroutine = null;
    private Coroutine radialCoroutine = null;

    public RectTransform spawnRect;

    Bullet bulletA;

    void Awake()
    {
        bulletA = new Bullet(
            "bullet_drop",
            new Color(0.6f, 0.4f, 0.7f),
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

                DoRadialAttack(randomPos);
                
                yield return new WaitForSeconds(0.6f);
            }
            
            yield return new WaitForSeconds(2.6f);

        }
    }

    private void DoRadialAttack(Vector3 spawnPos)
    {
        const int BULLET_COUNT = 14;
        
        for (int i = 0; i < BULLET_COUNT; i++)
        {
            
            AnimationCurve spd = new AnimationCurve();
            spd.AddKey(0f, 8.0f);
            spd.AddKey(1.0f, 1.2f);
            spd.AddKey(1.1f, 1.2f);
            spd.AddKey(1.5f, 1.2f);

            BulletSpawner.Instance.SpawnBullet(spawnPos, Mathf.Sin(Time.time) * 10.0f - 0f - 60f + 120f / BULLET_COUNT * i, bulletA, spd);
        }
    }

}
