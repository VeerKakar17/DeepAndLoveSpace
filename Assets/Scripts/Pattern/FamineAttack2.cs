using UnityEngine;
using System.Collections;

public class FamineAttack2 : MonoBehaviour
{
    private Coroutine boxCoroutine = null;
    private Coroutine radialCoroutine = null;

    public RectTransform spawnRect;
    private float BOX_SIZE = 4f;

    Bullet bulletA;

    void Awake()
    {
        bulletA = new Bullet(
            "bullet_drop",
            new Color(0.6f, 0.4f, 0.7f),
            0.12f,
            "no"
        );

        StartCoroutine(MoveBoxX());
        StartCoroutine(MoveBoxY());
        StartCoroutine(MoveBoxScale());
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
            SpawnHeart();
        }
    }
    private void SpawnHeart()
    {
        float y = Random.Range(-4f, 0f);
        int x = Random.Range(2, 3);
        x *= (Random.Range(0, 2) == 0 ? 1 : -1);
        HeartPieceManager.Instance.ActivateNextPiece(new Vector2(x, y));
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

    private IEnumerator MoveBoxX()
    {
        const float SECONDS = 4f;
        yield return StartCoroutine(GameManager.Instance.player.movementBox.updateXPos(1.5f-GameManager.Instance.player.movementBox.gameObject.transform.position.x, SECONDS / 2));
        while (true)
        {
            yield return StartCoroutine(GameManager.Instance.player.movementBox.updateXPos(-3f, SECONDS));
            yield return StartCoroutine(GameManager.Instance.player.movementBox.updateXPos(3f, SECONDS));
        }
    }

    private IEnumerator MoveBoxScale()
    {
        const float SECONDS = 5.1f;

        yield return StartCoroutine(GameManager.Instance.player.movementBox.setScale(1.2f * BOX_SIZE, SECONDS / 2));
        while (true)
        {
            yield return StartCoroutine(GameManager.Instance.player.movementBox.setScale(1.3f * BOX_SIZE, SECONDS));
            yield return StartCoroutine(GameManager.Instance.player.movementBox.setScale(1.2f * BOX_SIZE, SECONDS));
        }
    }

    private IEnumerator MoveBoxY()
    {
        const float SECONDS = 2.8f;
        yield return StartCoroutine(GameManager.Instance.player.movementBox.updateYPos(-2.85f - GameManager.Instance.player.movementBox.gameObject.transform.position.y, SECONDS / 2));

        while (true)
        {
            yield return StartCoroutine(GameManager.Instance.player.movementBox.updateYPos(0.75f, SECONDS));
            yield return StartCoroutine(GameManager.Instance.player.movementBox.updateYPos(-0.75f, SECONDS));
        }
    }
}
