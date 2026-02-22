using UnityEngine;
using System.Collections;

public class RadialPatternBow : MonoBehaviour
{
    private Coroutine boxCoroutine = null;
    private Coroutine radialCoroutine = null;
    private Coroutine homingCoroutine = null;
    Bullet arrowBullet;
    Bullet homingBullet;

    void Awake()
    {
        arrowBullet = new Bullet(
            2f,
            "bullet_arrow",
            Color.yellow,
            0.1f,
            "none"
        );
        homingBullet = new Bullet(
            4f,
            "bullet_sword",
            Color.yellow,
            0.1f,
            "none"
        );

        Debug.Log("Started");

        boxCoroutine = StartCoroutine(BoxMoveCoroutine());
        radialCoroutine = StartCoroutine(RadialCoroutine());
        homingCoroutine = StartCoroutine(HomingCoroutine());
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
            for (int i = 0; i < 2; i++)
            {
                DoRadialAttack(new Vector3(-2f, 2.5f, 1f));
                yield return new WaitForSeconds(1f);
            }
            for (int i = 0; i < 2; i++)
            {
                DoRadialAttack(new Vector3(2f, 2.5f, 1f));
                yield return new WaitForSeconds(1f);
            }
        }
    }

    private void DoRadialAttack(Vector3 spawnPos)
    {
        const int BULLET_COUNT = 51;

        for (int i = 0; i < BULLET_COUNT; i++)
        {
            BulletSpawner.Instance.SpawnBullet(spawnPos, 360 / BULLET_COUNT * i, arrowBullet);
        }
    }

    private IEnumerator HomingCoroutine()
    {
        Vector3 startPos = new Vector3(0f, 2.5f, 1f);
        while (true)
        {
            float waitTime = Random.Range(1f, 3f);
            startPos.x = Random.Range(-2f, 2f);

            yield return new WaitForSeconds(waitTime);

            float timescale = 1f;
            float offset = 1f;

            Vector3 PlayerPosition = GameManager.Instance.player.transform.position;
            PlayerPosition += new Vector3(Mathf.Sin(Time.time * timescale) * offset, 0);
            Vector3 direction = (PlayerPosition - startPos) / 2f;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            Debug.Log("Spawned Homing");
            BulletSpawner.Instance.SpawnBullet(startPos, angle+90, homingBullet);
            
            yield return null;
        }
    }
}
