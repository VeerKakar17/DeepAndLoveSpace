using UnityEngine;
using System.Collections;

public class WarFireAttack : MonoBehaviour
{
    private Coroutine boxCoroutine = null;
    private Coroutine radialCoroutine = null;
    Bullet bulletA;

    void Awake()
    {
        bulletA = new Bullet(
            "bullet_big",
            Color.red,
            0.1f,
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
        while (true)
        {
            for (int i = 0; i < 5; i++)
            {
                DoRadialAttack(new Vector3(-2f, 2.5f, 1f));
                yield return new WaitForSeconds(0.2f);
            }
            yield return new WaitForSeconds(1.2f);
            for (int i = 0; i < 5; i++)
            {
                DoRadialAttack(new Vector3(2f, 2.5f, 1f));
                yield return new WaitForSeconds(0.2f);
            }
            yield return new WaitForSeconds(1.2f);
        }
    }

    private void DoRadialAttack(Vector3 spawnPos)
    {
        const int BULLET_COUNT = 8;

        for (int i = 0; i < BULLET_COUNT; i++)
        {
            AnimationCurve spd = new AnimationCurve();
            spd.AddKey(0f, 1.0f);
            spd.AddKey(1.5f, 0.0f);
            spd.AddKey(2.0f, 3.6f);
            
            BulletSpawner.Instance.SpawnBullet(spawnPos, Mathf.Sin((float)Time.time) * 360.0f + 360.0f / BULLET_COUNT * i, bulletA, spd);
        }
    }

}
