using UnityEngine;
using System.Collections;

public class FamineAttack1 : MonoBehaviour
{
    private Coroutine radialCoroutine = null;

    public RectTransform spawnRect;

    Bullet bulletA;

    void Awake()
    {
        bulletA = new Bullet(
            "bullet_big",
            new Color(0.5f, 0.4f, 0.8f),
            0.12f,
            "famine_a_spawner"
        );

        radialCoroutine = StartCoroutine(RadialCoroutine());
    }

    private IEnumerator RadialCoroutine()
    {
        int num_spawns = 0;
        while (true)
        {

            Spawn();
            num_spawns++;
            if (num_spawns == 4)
            {
                HeartPieceManager.Instance.ActivateNextPiece();
            }
            
            yield return new WaitForSeconds(2.8f);

        }
    }

    private void Spawn()
    {
        const int BULLET_COUNT = 1;

        for (int i = 0; i < BULLET_COUNT; i++)
        {
            AnimationCurve spd = new AnimationCurve();
            spd.AddKey(0f, 0.0f);
            spd.AddKey(0.5f, 0.1f);
            spd.AddKey(1.5f, 0.8f);
            spd.AddKey(2.0f, 1.4f);

            // position is random position inside of spawn rect
            Vector3 randomPos = spawnRect.position
                + spawnRect.right * Random.Range(-spawnRect.rect.width / 2f, spawnRect.rect.width / 2f)
                + spawnRect.up * Random.Range(-spawnRect.rect.height / 2f, spawnRect.rect.height / 2f);

            BulletSpawner.Instance.SpawnBullet(randomPos, 0.0f, bulletA, spd);
        }
    }

}
