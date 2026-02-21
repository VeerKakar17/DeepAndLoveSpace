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

    [Header("Flash Settings")]
    public int flashCount = 2;
    public float flashSpeed = 0.15f;

    [Header("Colors")]
    public Color lightColor = Color.white;
    public Color mediumDarkColor = new Color(0.5f, 0.5f, 0.5f);
    public Color fullDarkColor = new Color(0.2f, 0.2f, 0.2f);

    private SpriteRenderer sr;
    public bool isDark = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = lightColor;

        StartCoroutine(CycleRoutine());
    }

    IEnumerator CycleRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(lightDuration);

            float randomDelay = Random.Range(randomDelayMin, randomDelayMax);
            yield return new WaitForSeconds(randomDelay);

            yield return StartCoroutine(FlashTransition(lightColor, mediumDarkColor));

            sr.color = fullDarkColor;
            isDark = true;

            yield return new WaitForSeconds(darkDuration);

            yield return StartCoroutine(FlashTransition(fullDarkColor, mediumDarkColor));

            sr.color = lightColor;
            isDark = false;
        }
    }

    IEnumerator FlashTransition(Color from, Color flash)
    {
        for (int i = 0; i < flashCount; i++)
        {
            sr.color = flash;
            yield return new WaitForSeconds(flashSpeed);

            sr.color = from;
            yield return new WaitForSeconds(flashSpeed);
        }
    }
}