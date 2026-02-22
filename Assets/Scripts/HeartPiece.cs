using UnityEngine;
using System.Collections;

public class HeartPiece : MonoBehaviour
{
    public int pieceIndex;

    [HideInInspector]
    public bool isSnapped = false;

    private HeartPieceManager manager;
    private SpriteRenderer sr;

    Vector3 originalScale;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
    }

    public void Initialize(HeartPieceManager m, int index)
    {
        manager = m;
        pieceIndex = index;
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void OnSnapped()
    {
        if (isSnapped) return;

        isSnapped = true;
        manager.OnPieceSnapped(this);
    }

    public void PlayCompleteAnimation()
    {
        StartCoroutine(GrowAndGlow());
    }

    IEnumerator GrowAndGlow()
    {
        float duration = 0.4f;
        float t = 0;

        Vector3 targetScale = originalScale * 1.25f;
        Color startColor = sr.color;
        Color glowColor = Color.white;

        while (t < duration)
        {
            float lerp = t / duration;

            transform.localScale = Vector3.Lerp(originalScale, targetScale, lerp);
            sr.color = Color.Lerp(startColor, glowColor, lerp);

            t += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
        sr.color = glowColor;
    }
}