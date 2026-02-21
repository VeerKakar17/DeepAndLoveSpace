using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Time Settings")]
    public float slowMoScale = 0.3f;
    public float slowMoDuration = 2f;

    private bool isPaused = false;
    private bool isSlowMo = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();

        if (Input.GetKeyDown(KeyCode.LeftShift))
            TriggerSlowMotion();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    public void TriggerSlowMotion()
    {
        if (!isSlowMo)
            StartCoroutine(SlowMotionRoutine());
    }

    private System.Collections.IEnumerator SlowMotionRoutine()
    {
        isSlowMo = true;

        Time.timeScale = slowMoScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(slowMoDuration);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        isSlowMo = false;
    }
}