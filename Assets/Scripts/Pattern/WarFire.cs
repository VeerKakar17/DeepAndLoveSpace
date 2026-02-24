using UnityEngine;
using System.Collections;

public class WarFireAttack : MonoBehaviour
{
    private Coroutine boxCoroutine = null;
    private Coroutine radialCoroutine = null;
    Bullet bulletA;
    Bullet bulletB;
    Bullet bulletC;

    void Awake()
    {
        bulletA = new Bullet(
            "bullet_big",
            new Color(0.91f, 0.1f, 0.1f),
            0.15f,
            "none"
        );
        bulletB = new Bullet(
            "bullet_base",
            new Color(1f, 0.5f, 0.4f),
            0.1f,
            "none"
        );
        bulletC = new Bullet(
            "bullet_dot",
            new Color(1f, 0.77f, 0.67f),
            0.05f,
            "none"
        );

        Debug.Log("Started");

        boxCoroutine = StartCoroutine(BoxMoveCoroutine());
        radialCoroutine = StartCoroutine(RadialCoroutine());
    }

    public void Update()
    {
    }

    private IEnumerator BoxMoveCoroutine()
    {
        while (true)
        {
            yield return GameManager.Instance.player.movementBox.updateScale(0.9f, 2f);
            yield return GameManager.Instance.player.movementBox.updateScale(1.1f, 2f);
        }
    }

    private IEnumerator RadialCoroutine()
    {
        const int NUM_REPS = 2;
        int numElapsed = 0;
        while (true)
        {
            for (int i = 0; i < 5; i++)
            {
                DoRadialAttack(new Vector3(-2f, 2.5f, 1f));
                yield return new WaitForSeconds(0.23f);
            }
            yield return new WaitForSeconds(1.2f);
            if (numElapsed == 1)
            {
                HeartPieceManager.Instance.ActivateNextPiece();
            }
            for (int i = 0; i < 5; i++)
            {
                DoRadialAttack(new Vector3(2f, 2.5f, 1f));
                yield return new WaitForSeconds(0.23f);
            }
            yield return new WaitForSeconds(1.2f);
            numElapsed++;
        }
    }

    private void DoRadialAttack(Vector3 spawnPos)
    {
        const int BULLET_COUNT = 10;

        for (int i = 0; i < BULLET_COUNT; i++)
        {
            AnimationCurve spd = new AnimationCurve();
            spd.AddKey(0f, 1.0f);
            spd.AddKey(1.0f, 0.0f);
            spd.AddKey(1.5f, 4.6f);
            
            BulletSpawner.Instance.SpawnBullet(spawnPos, Mathf.Sin((float)Time.time) * 360.0f + 360.0f / BULLET_COUNT * i, bulletA, spd);
            
            AnimationCurve spd2 = new AnimationCurve();
            spd2.AddKey(0f, 0.95f);
            spd2.AddKey(1.1f, 0.0f);
            spd2.AddKey(1.5f, 4.4f);
            
            BulletSpawner.Instance.SpawnBullet(spawnPos, Mathf.Sin((float)Time.time + 0.008f) * 360.0f + 360.0f / BULLET_COUNT * i, bulletB, spd2);
            BulletSpawner.Instance.SpawnBullet(spawnPos, Mathf.Sin((float)Time.time - 0.008f) * 360.0f + 360.0f / BULLET_COUNT * i, bulletB, spd2);
            
            AnimationCurve spd3 = new AnimationCurve();
            spd3.AddKey(0f, 0.9f);
            spd3.AddKey(1.2f, 0.0f);
            spd3.AddKey(1.5f, 4.2f);
            
            BulletSpawner.Instance.SpawnBullet(spawnPos, Mathf.Sin((float)Time.time + 0.016f) * 360.0f + 360.0f / BULLET_COUNT * i, bulletC, spd3);
            BulletSpawner.Instance.SpawnBullet(spawnPos, Mathf.Sin((float)Time.time - 0.016f) * 360.0f + 360.0f / BULLET_COUNT * i, bulletC, spd3);
        }
    }

}
