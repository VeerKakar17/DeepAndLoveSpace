using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform patternContainer;

    [Header("Time Settings")]
    public float slowMoScale = 0.3f;
    public float slowMoDuration = 2f;

    private bool isPaused = false;
    private bool isSlowMo = false;


    public List<GameEvent> events = new List<GameEvent>();
    public GameEvent currentEvent;

    public PlayerMovement player;
    public BossControl bossControl;


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

    [Header("Dialogue Settings")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI dialogueTextPlayer;
    [SerializeField] private GameObject pressZIndicator;
    private Coroutine pressZCoroutine = null;
        
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

        GameObject patternObj = Resources.Load<GameObject>("BigBowRadialPrefab");

        events.Add(new DialogueEvent("CONQUEST: NEIGH. I will hear this peasant’s pleas first. Speak, human.", true));
        events.Add(new DialogueEvent("My king, you wish to destroy humanity because you have not yet seen the joys of love that our species has to offer. Despite our mortality, the beauties of humanity are beyond your perception.", false));
        events.Add(new DialogueEvent("CONQUEST: You dare imply that I am ignorant? You… wretched thing?", true));
        events.Add(new DialogueEvent("Fear not, my king. I will show you what love is.", false));
        
        events.Add(new PatternEvent(patternObj));
        
        events.Add(new DialogueEvent("Example Attack Incoming", true));
        
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

    public void SetDialogue(string dialogue, bool isEnemy, bool addPressZ = false)
    {
        TextMeshProUGUI curr;
        if (isEnemy)
        {
            curr = dialogueText;
        } else
        {
            curr = dialogueTextPlayer;
        }
        curr.text = dialogue;
        if (addPressZ)
        {
            // Convert from TMP local space to world space

            pressZIndicator.SetActive(true);
            TMP_CharacterInfo lastChar = curr.textInfo.characterInfo[curr.textInfo.characterCount - 1];
            Vector3 worldBottomRight = curr.transform.TransformPoint(lastChar.bottomRight);
            pressZIndicator.transform.position = new Vector3(pressZIndicator.transform.position.x, worldBottomRight.y-55f, pressZIndicator.transform.position.z);
            
            pressZCoroutine = StartCoroutine(PressZCoroutine());
        }
    }

    public void ClearDialogue()
    {
        dialogueText.text = "";
        dialogueTextPlayer.text = "";
        pressZIndicator.SetActive(false);
        if (pressZCoroutine != null)
        {
            StopCoroutine(pressZCoroutine);
            pressZCoroutine = null;
        }
    }

    private IEnumerator PressZCoroutine()
    {
        const float MAX_ALPHA = 0.6f;
        const float MIN_ALPHA = 0.2f;
        const float TIME_TO_FADE = 0.8f;

        TextMeshProUGUI pressZTmp = pressZIndicator.GetComponent<TextMeshProUGUI>();

        while (true)
        {
            Color currentColor = pressZTmp.color;
            currentColor.a = MAX_ALPHA;
            pressZTmp.color = currentColor;

            float elapsed = 0f;
            while (elapsed < TIME_TO_FADE)
            {
                float t = elapsed / TIME_TO_FADE;
                currentColor.a = Mathf.Lerp(MAX_ALPHA, MIN_ALPHA, t);
                pressZTmp.color = currentColor;

                elapsed += Time.deltaTime;
                yield return null;
            }

            elapsed = 0f;
            while (elapsed < TIME_TO_FADE)
            {
                float t = elapsed / TIME_TO_FADE;
                currentColor.a = Mathf.Lerp(MIN_ALPHA, MAX_ALPHA, t);
                pressZTmp.color = currentColor;

                elapsed += Time.deltaTime;
                yield return null;
            }

            currentColor.a = MIN_ALPHA;
            pressZTmp.color = currentColor;
        }
    }
}