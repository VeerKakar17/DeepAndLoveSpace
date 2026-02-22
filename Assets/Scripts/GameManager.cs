using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.InputSystem;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Time Settings")]
    public float slowMoScale = 0.3f;
    public float slowMoDuration = 2f;

    private bool isPaused = false;
    private bool isSlowMo = false;


    public List<GameEvent> events = new List<GameEvent>();
    public GameEvent currentEvent;


    public GameObject bossHeartSnapTarget;
    public GameObject heartPrefab;
    public void SpawnHeart(Vector3 position)
    {
        
        GameObject heartObj = Instantiate(heartPrefab, position, Quaternion.identity, transform);

        SnapToTarget s = heartObj.GetComponent<SnapToTarget>();
        s.snapTarget = bossHeartSnapTarget.transform;
    }

    [Header("Health Settings")]
    public const int MAX_LIVES = 3;
    public int lives = MAX_LIVES;
        
    public void StartNextEvent()
    {
        if (events.Count > 0)
        {
            currentEvent = events[0];
            events.RemoveAt(0);
            currentEvent.StartEvent();

            Debug.Log("Started next event. Remaining events: " + events.Count);
        }
        else
        {
            Debug.Log("No more events to start.");
        }
    }

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

    public void LoadSampleLevel() {
        lives = MAX_LIVES;
        events.Clear();

        events.Add(new DialogueEvent("Example Dialogue Hello"));

        Attack e1 = new Attack();
        for (int i = 0; i < 24; i++) {
            float angle = Random.Range(-25f, 25f);
            float speed = Random.Range(2.5f, 3.5f);
            e1.addEntry(i * 0.15f, new BulletEntry(new Bullet(angle, speed, "bullet_arrow", Color.yellow, 0.1f, "none")));
        }
        for (int i = 25; i < 35; i++) {
            float angle = Random.Range(-15f, 15f);
            float speed = Random.Range(2.0f, 2.5f);
            e1.addEntry(i * 0.15f, new BulletEntry(new Bullet(angle, speed, "bullet_bigarrow", Color.yellow, 0.2f, "none")));
        }

        Attack e2 = new Attack();
        for (int i = 0; i < 48; i++) {
            float angle = Random.Range(-15f, 15f);
            Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
            e2.addEntry(i * 0.25f, new BulletEntry(new Bullet(angle, 2.2f, "bullet_big", Color.red, 0.25f, "none")));

            if (i == 4) {
                e2.addEntry(i * 0.25f + 0.125f, new HeartEntry(new Vector3(0, -1.0f, 0)));
            }
        }

        events.Add(new AttackEvent(e2));

        events.Add(new DialogueEvent("Example Attack Incoming"));
        
        events.Add(new AttackEvent(e1));
    }

    public void Start() {
        LoadSampleLevel();
        StartNextEvent();
    }

    void Update()
    {

        bool keyPressedThisFrame = false;
        foreach (var key in Keyboard.current.allKeys)
        {
            if (key.wasPressedThisFrame)
            {
                keyPressedThisFrame = true;

                if (key == Keyboard.current.spaceKey)
                {
                    // TogglePause();
                }

                if (key == Keyboard.current.leftShiftKey)
                {
                    //TriggerSlowMotion();

                }
            }
        }

        currentEvent?.UpdateEvent();
    }

    public void LoseLife()
    {
        lives--;
        Debug.Log("Lives: " + lives + " left.");
        if (lives < 0)
        {
            Debug.Log("Player lost");
            BulletSpawner.Instance.ResetBullets();
            LoadSampleLevel();
        }
    }
}