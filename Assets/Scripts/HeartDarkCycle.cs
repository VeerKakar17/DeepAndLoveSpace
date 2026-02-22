using UnityEngine;
using System.Collections;

public class HeartDarkCycle : MonoBehaviour
{
    [Header("Timing")]
    public float lightDuration = 7f;
    public float darkDuration = 5f;

    [Header("Random Delay")]
    public float randomDelayMin = 5f;
    public float randomDelayMax = 9f;

    [Header("Fade Settings")]
    public int flashCount = 2;
    public float fadeDuration = 0.3f;

    [Header("Colors")]
    public Color lightColor = Color.white;
    public Color mediumDarkColor = new Color(0.5f, 0.5f, 0.5f);
    public Color fullDarkColor = new Color(0.2f, 0.2f, 0.2f);
    
    [Header("Floating Motion")]
    public float circleRadius = 0.1f;
    public float circleSpeed = 1f;

    private SpriteRenderer sr;
    private Vector3 startPosition;
    private float circleTime = 0f;
    public bool isDark = false;

    public bool isPaused = false;

    public void PauseCycle()
    {
        isPaused = true;
    }

    public void ResumeCycle()
    {
        isPaused = false;
        
        StopAllCoroutines();
        StartCoroutine(CycleRoutine());
    }

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = lightColor;

        startPosition = transform.position;

        StartCoroutine(CycleRoutine());
    }
    void Update()
    {
        HandleCircularMotion();
    }

    void HandleCircularMotion()
    {

        if (isPaused) return;

        circleTime += Time.deltaTime * circleSpeed;

        float x = Mathf.Cos(circleTime) * circleRadius;
        float y = Mathf.Sin(circleTime) * circleRadius;

        transform.position = startPosition + new Vector3(x, y, 0);
    }
    IEnumerator CycleRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(lightDuration);

            float randomDelay = Random.Range(randomDelayMin, randomDelayMax);
            yield return new WaitForSeconds(randomDelay);

            yield return StartCoroutine(FadeFlash(lightColor, mediumDarkColor));

            yield return StartCoroutine(FadeTo(fullDarkColor));
            
            while (isPaused) {
                yield return new WaitForSeconds(1.0f);
            }

            isDark = true;

            yield return new WaitForSeconds(darkDuration);

            yield return StartCoroutine(FadeFlash(fullDarkColor, mediumDarkColor));

            yield return StartCoroutine(FadeTo(lightColor));
            
            while (isPaused) {
                yield return new WaitForSeconds(1.0f);
            }

            isDark = false;
        }
    }

    IEnumerator FadeFlash(Color baseColor, Color flashColor)
    {
        for (int i = 0; i < flashCount; i++)
        {
            yield return StartCoroutine(FadeBetween(baseColor, flashColor));
            yield return StartCoroutine(FadeBetween(flashColor, baseColor));
        }
    }

    IEnumerator FadeTo(Color target)
    {
        yield return StartCoroutine(FadeBetween(sr.color, target));
    }

    IEnumerator FadeBetween(Color start, Color end)
    {
        float time = 0;

        while (time < fadeDuration)
        {
            while (isPaused) {
                yield return new WaitForSeconds(1.0f);
            }
            sr.color = Color.Lerp(start, end, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        sr.color = end;
    }

    public void FixColor()
    {
        if (sr == null) return;
        if (isDark)
        {
            sr.color = fullDarkColor;
        }
        else
        {
            sr.color = lightColor;
        }
    }
}