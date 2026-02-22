using UnityEngine;
using System.Collections;

public class FamineAttack1 : MonoBehaviour
{
    void Awake()
    {
        MoveBox();
    }

    private void MoveBox()
    {
        StartCoroutine(MoveBoxX());
        StartCoroutine(MoveBoxY());
        StartCoroutine(MoveBoxScale());
    }

    private IEnumerator MoveBoxX()
    {
        const float SECONDS = 4f;
        yield return StartCoroutine(GameManager.Instance.player.movementBox.updateXPos(1.5f, SECONDS / 2));
        while (true)
        {
            yield return StartCoroutine(GameManager.Instance.player.movementBox.updateXPos(-3f, SECONDS));
            yield return StartCoroutine(GameManager.Instance.player.movementBox.updateXPos(3f, SECONDS));
        }
    }

    private IEnumerator MoveBoxY()
    {
        const float SECONDS = 2.8f;
        yield return StartCoroutine(GameManager.Instance.player.movementBox.updateYPos(0.5f, SECONDS / 2));
        while (true)
        {
            yield return StartCoroutine(GameManager.Instance.player.movementBox.updateYPos(-1f, SECONDS));
            yield return StartCoroutine(GameManager.Instance.player.movementBox.updateYPos(1f, SECONDS));
        }
    }

    private IEnumerator MoveBoxScale()
    {
        const float SECONDS = 5.1f;
        while (true)
        {
            yield return StartCoroutine(GameManager.Instance.player.movementBox.updateScale(1.2f, SECONDS));
            yield return StartCoroutine(GameManager.Instance.player.movementBox.updateScale(0.8f, SECONDS));
        }
    }
}
