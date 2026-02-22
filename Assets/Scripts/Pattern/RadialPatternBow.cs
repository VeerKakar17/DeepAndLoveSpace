using UnityEngine;
using System.Collections;

public class RadialPatternBow : MonoBehaviour
{
    private Coroutine boxCoroutine = null;
    private Coroutine radialCoroutine = null;
    Bullet arrowBullet;

    void Awake()
    {
        arrowBullet = new Bullet(
            2f,
            "bullet_arrow",
            Color.yellow,
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

    public void End()
    {
        StopCoroutine(boxCoroutine);
        StopCoroutine(radialCoroutine);
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
        while (true)
        {
            DoRadialAttack(new Vector3(-2f, 2.5f, 1f));
            yield return new WaitForSeconds(1f);
        }
    }

    private void DoRadialAttack(Vector3 spawnPos)
    {
        const int BULLET_COUNT = 60;

        for (int i = 0; i < BULLET_COUNT; i++)
        {
            BulletSpawner.Instance.SpawnBullet(spawnPos, 360 / BULLET_COUNT * i, arrowBullet);
        }
    }
}
