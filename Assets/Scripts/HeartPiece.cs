using UnityEngine;
using System.Collections;

public class HeartPiece : MonoBehaviour
{
    public int pieceIndex;

    [HideInInspector]
    public bool isSnapped = false;
    public bool isDark = false;

    private HeartPieceManager manager;
    private SpriteRenderer sr;

    Vector3 originalScale;
    public float rainbowSpeed = .005f;
    public float glowIntensity = 1.0f;
    private float hue = 0f;
    private Color baseColor;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        baseColor = sr.color;
        
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

        originalScale = transform.localScale;
        Vector3 targetScale = originalScale * 1.25f;

        while (t < duration)
        {
            float lerp = t / duration;

            transform.localScale = Vector3.Lerp(originalScale, targetScale, lerp);

            t += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;

        while (true)
        {
            hue += Time.deltaTime * rainbowSpeed;
            if (hue > 1f) hue -= 1f;

            float pulse = 1f + Mathf.Sin(Time.time * 3f) * 0.1f;
            transform.localScale = originalScale * 1.25f * pulse;

            if (!isDark)
            {
                sr.color = Color.HSVToRGB(hue, .7f, glowIntensity);
            }

            yield return null;
        }
    }
}