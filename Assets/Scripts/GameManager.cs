using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{

    public GameObject StartingScene;
    public GameObject IntroScene;

    public GameObject EndingScene;

    public static GameManager Instance;

    public Transform patternContainer;

    public HeartPieceManager heartPieceManager;

    [Header("Time Settings")]
    public float slowMoScale = 0.3f;
    public float slowMoDuration = 2f;

    private bool isPaused = false;
    private bool isSlowMo = false;

    public int currentLevel = -1;

    public List<GameEvent> events = new List<GameEvent>();
    public GameEvent currentEvent;

    public PlayerMovement player;
    public BossControl bossControl;
    public List<GameObject> currStagePatterns;

    [Header("Health Settings")]
    public const int MAX_LIVES = 3;
    public int lives = MAX_LIVES;

    [Header("Dialogue Settings")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI dialogueTextPlayer;
    [SerializeField] private GameObject pressZIndicator;
    [SerializeField] private Image blackOverlay;
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
            currentEvent = new PatternEvent(currStagePatterns[Random.Range(0, currStagePatterns.Count)]);
            currentEvent.StartEvent();
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // DontDestroyOnLoad(patternContainer.gameObject);
            // DontDestroyOnLoad(heartPieceManager.gameObject);
            // DontDestroyOnLoad(dialogueText.gameObject);
            // DontDestroyOnLoad(dialogueTextPlayer.gameObject);
            // DontDestroyOnLoad(pressZIndicator.gameObject);
            // DontDestroyOnLoad(player.gameObject);
            // DontDestroyOnLoad(player.movementBox.gameObject);
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

    public IEnumerator LoadLevelConquest()
    {
        yield return FadeToBlack(1f);
        yield return UnLoadLevel();
        

        events.Clear();

        currStagePatterns = new List<GameObject>();

        GameObject patternObj = Resources.Load<GameObject>("BigBowRadialPrefab");
        GameObject patternObj2 = Resources.Load<GameObject>("BigBowPrefabTypeA");
        currStagePatterns.Add(patternObj);
        currStagePatterns.Add(patternObj2);

        
        events.Add(new DialogueEvent("CONQUEST: NEIGH. I will hear this peasant�s pleas first. Speak, human.", true));
        events.Add(new DialogueEvent("My king, you wish to destroy humanity because you have not yet seen the joys of love that our species has to offer. Despite our mortality, the beauties of humanity are beyond your perception.", false));
        events.Add(new DialogueEvent("CONQUEST: You dare imply that I am ignorant? You� wretched thing?", true));
        events.Add(new DialogueEvent("Fear not, my king. I will show you what love is.", false));

        events.Add(new PatternEvent(patternObj));

        events.Add(new PatternEvent(patternObj2));

        yield return SceneManager.LoadSceneAsync("Main Scene Conquest", LoadSceneMode.Additive);

        LoadLevel();
        Debug.Log("Finished loading scene, starting events.");

        StartNextEvent();
        Debug.Log("Started first event.");
        yield return FadeFromBlack(1f);

    }


    public IEnumerator LoadLevelWar()
    {
        yield return FadeToBlack(1f);
        yield return UnLoadLevel();

        events.Clear();

        currStagePatterns = new List<GameObject>();

        events.Add(new DialogueEvent("WAR: I will chop off all your limbs if you disappoint me. Speak now, human!\r\n", true));
        events.Add(new DialogueEvent("Calm your fury. Through your neverending violence you have yet to consider the tender embrace of love. I will teach you.\r\n", false));
        events.Add(new DialogueEvent("WAR: This mere human dares to challenge me? Prepare yourself to be impaled by my sword!\r\n", true));

        GameObject patternObj3 = Resources.Load<GameObject>("WarAttackPrefab3");
        events.Add(new PatternEvent(patternObj3));
        currStagePatterns.Add(patternObj3);

        GameObject patternObj = Resources.Load<GameObject>("WarAttackPrefab2");
        events.Add(new PatternEvent(patternObj));
        currStagePatterns.Add(patternObj);

        GameObject patternObj2 = Resources.Load<GameObject>("WarAttackPrefab1");
        events.Add(new PatternEvent(patternObj2));
        currStagePatterns.Add(patternObj2);

        yield return SceneManager.LoadSceneAsync("Main Scene War", LoadSceneMode.Additive);

        LoadLevel();
        Debug.Log("Finished loading scene, starting events.");

        StartNextEvent();
        Debug.Log("Started first event.");
        yield return FadeFromBlack(1f);

    }


    public IEnumerator LoadLevelFamine()
    {
        yield return FadeToBlack(1f);
        yield return UnLoadLevel();

        events.Clear();

        currStagePatterns = new List<GameObject>();

        events.Add(new DialogueEvent("Famine: Hi little human! Sorry about my brothers, they can be so overbearing at times.", true));
        events.Add(new DialogueEvent("Will you consider sparing humanity?", false));
        events.Add(new DialogueEvent("Famine: Hmmm… I don’t want to! But you are so cute… I just really want to… gobble you up!", true));

        yield return SceneManager.LoadSceneAsync("Main Scene Famine", LoadSceneMode.Additive);

        GameObject patternObj2 = Resources.Load<GameObject>("FamineAttackPrefab2");
        events.Add(new PatternEvent(patternObj2));
        currStagePatterns.Add(patternObj2);

        GameObject patternObj = Resources.Load<GameObject>("FamineAttackPrefab1");
        events.Add(new PatternEvent(patternObj));
        currStagePatterns.Add(patternObj);


        LoadLevel();
        Debug.Log("Finished loading scene, starting events.");

        StartNextEvent();
        Debug.Log("Started first event.");
        yield return FadeFromBlack(1f);

    }

    public IEnumerator LoadLevelDeath()
    {
        yield return FadeToBlack(1f);
        yield return UnLoadLevel();

        events.Clear();

        currStagePatterns = new List<GameObject>();

        events.Add(new DialogueEvent("Death: I see you have managed to convince my brothers to spare you somehow. Beware, for I am not as easily convinced! Death will end all suffering.", true));
        events.Add(new DialogueEvent("Me: Yes, life brings about much suffering, but it also brings about much joy. I will survive you and show you what true love is!", false));
        events.Add(new DialogueEvent("Death: Let us see if you can escape from my clutches. Entertain me!", true));

        yield return SceneManager.LoadSceneAsync("Main Scene Death", LoadSceneMode.Additive);

        GameObject patternObj = Resources.Load<GameObject>("DeathAttack1");
        events.Add(new PatternEvent(patternObj));
        currStagePatterns.Add(patternObj);

        GameObject patternObj2 = Resources.Load<GameObject>("DeathAttack2");
        events.Add(new PatternEvent(patternObj2));
        currStagePatterns.Add(patternObj2);

        LoadLevel();
        Debug.Log("Finished loading scene, starting events.");

        StartNextEvent();
        Debug.Log("Started first event.");
        yield return FadeFromBlack(1f);
    }



    public void LoadLevel()
    {
        heartPieceManager.heartSnapTargetGroupTransform.SetParent(bossControl.HeartParent, false);
        heartPieceManager.ResetPieces();
        lives = MAX_LIVES;
    }

    public IEnumerator UnLoadLevel()
    {
        heartPieceManager.heartSnapTargetGroupTransform.SetParent(heartPieceManager.transform, false);

        // unload all scenes except the active one
        Scene active = SceneManager.GetActiveScene();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene s = SceneManager.GetSceneAt(i);

            if (s != active)
                yield return SceneManager.UnloadSceneAsync(s);
        }
    }

    public void Start()
    {

        StartingScene.SetActive(true);

        events.Clear();
        events.Add(new WaitForZEvent());

        events.Add(new DialogueEvent("We are the four horsemen of the apocalypse and we come to destroy this world. We will give humanity one last chance to prove they are worthy of redemption.", true));
        events.Add(new DialogueEvent("I hold deep love for humanity. I will risk my life to convince them to spare us.", false));

        events.Add(new NextLevelEvent());
        StartNextEvent();

    }

    public void LoadLevelFromNumber(int i)
    {

        IntroScene.SetActive(false);
        StartingScene.SetActive(false);

        if (i == 0)
        {
            StartCoroutine(LoadLevelConquest());
        }
        else if (i == 1)
        {
            StartCoroutine(LoadLevelWar());
        }
        else if (i == 2)
        {
            StartCoroutine(LoadLevelFamine());
        }
        else if (i == 3)
        {
            StartCoroutine(LoadLevelDeath());
        }
        else
        {
            Debug.Log("No more levels to load. Ending game.");
            StartingScene.SetActive(false);
            EndingScene.SetActive(true);
        }
    }

    public void OnCompleteHeart()
    {
        BulletSpawner.Instance.ResetBullets();
        List<GameObject> ToRemove = new List<GameObject>();
        foreach (Transform go in patternContainer)
        {
            ToRemove.Add(go.gameObject);
        }
        foreach (var go in ToRemove)
        {
            Destroy(go);
        }

        //check if dark (more than 3 dark pieces)
        bool isDark = heartPieceManager.darkCount > 3;

        if (currentLevel == 0)
        {
            if (isDark)
            {
                StartCoroutine(ConquestDarkDialogue());
            }
            else
            {
                StartCoroutine(ConquestLightDialogue());
            }
        }
        else if (currentLevel == 1)
        {
            if (isDark)
            {
                StartCoroutine(WarDarkDialogue());
            }
            else
            {
                StartCoroutine(WarLightDialogue());
            }
        }
        else if (currentLevel == 2)
        {
            if (isDark)
            {
                StartCoroutine(FamineDarkDialogue());
            }
            else
            {
                StartCoroutine(FamineLightDialogue());
            }
        }
         else if (currentLevel == 3)
         {
            if (isDark)
            {
                StartCoroutine(DeathDarkDialogue());
            }
            else
            {
                StartCoroutine(DeathLightDialogue());
            }
         }
    }

    public void StartNextLevel()
    {
        currentLevel++;
        LoadLevelFromNumber(currentLevel);
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

        //BulletSpawner.Instance.ResetBullets();


        SoundManager.Instance.Play("tan1", 0.8f, 0.75f, 0.1f, true);

        if (lives < 0)
        {
            Debug.Log("Player lost");
            BulletSpawner.Instance.ResetBullets();
        }
    }

    public void SetDialogue(string dialogue, bool isEnemy, bool addPressZ = false)
    {
        TextMeshProUGUI curr;
        if (isEnemy)
        {
            curr = dialogueText;
        }
        else
        {
            curr = dialogueTextPlayer;
        }
        curr.text = dialogue;
        curr.ForceMeshUpdate();
        if (addPressZ)
        {
            pressZIndicator.SetActive(true);

            if (isEnemy)
            {
                TMP_CharacterInfo lastChar = curr.textInfo.characterInfo[dialogue.Length - 1];
                Vector3 worldBottomRight = curr.transform.TransformPoint(lastChar.bottomRight);
                pressZIndicator.transform.position = new Vector3(pressZIndicator.transform.position.x, worldBottomRight.y - 60f, pressZIndicator.transform.position.z);
            }
            else
            {
                TMP_CharacterInfo firstChar = curr.textInfo.characterInfo[0];
                Vector3 worldTopRight = curr.transform.TransformPoint(firstChar.topRight);
                pressZIndicator.transform.position = new Vector3(pressZIndicator.transform.position.x, worldTopRight.y + 15f, pressZIndicator.transform.position.z);
            }

            pressZCoroutine = StartCoroutine(PressZCoroutine());
        }
    }

    public void ClearDialogue()
    {
        Debug.Log("test");
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
    public IEnumerator FadeOverlay(float targetAlpha, float duration)
    {
        float startAlpha = blackOverlay.color.a;
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;

            Color c = blackOverlay.color;
            c.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            blackOverlay.color = c;

            time += Time.deltaTime;
            yield return null;
        }

        Color final = blackOverlay.color;
        final.a = targetAlpha;
        blackOverlay.color = final;
    }

    public IEnumerator ConquestLightDialogue()
    {
        events.Clear();
        events.Add(new PostBattleDialogueEvent(
            "Conquest: Very well human, I suppose I will grant you my clemency. I have been elucidated to this curious prospect of affection. As a reward I shall grant you a title, castle, servants, and plenty of gold. ",
            true
        ));
        events.Add(new PostBattleDialogueEvent(
            "I have no need for those material riches. All I request of you is to permit this lowly one to be your prince.",
            false
        ));
        events.Add(new PostBattleDialogueEvent(
            "Conquest: Then this noble one shall accept. ",
            true
        ));
        events.Add(new NextLevelEvent());
        
        StartNextEvent();
        yield return FadeToBlack(1f);
        yield break;
    }
    public IEnumerator ConquestDarkDialogue()
    {
        events.Clear();
        events.Add(new PostBattleDialogueEvent(
            "Conquest: You have taught me the pleasures of depraved longing. I shall take away all your possessions so that you have no other choice but to depend on me. You belong to me now.",
            true
        ));
        events.Add(new PostBattleDialogueEvent(
            "But-",
            false
        ));
        events.Add(new PostBattleDialogueEvent(
            "Conquest: Shhh my little peasant. You must call me Master from henceforth. ",
            true
        ));
        events.Add(new NextLevelEvent());
        
        StartNextEvent();
        yield return FadeToBlack(1f);
        yield break;
    }

    public IEnumerator WarLightDialogue()
    {
        events.Clear();
        events.Add(new PostBattleDialogueEvent(
            "War: I-its not like I like you or anything baka! Here’s a whole treasury of precious weapons. D-don’t get the wrong idea, I just found them lying around and wanted to get rid of them.",
            true
        ));
        events.Add(new PostBattleDialogueEvent(
            "*smiles* Thank you for the gifts, but what I really want is your hand in marriage.",
            false
        ));
        events.Add(new PostBattleDialogueEvent(
            "War: WHAT! F-fine BAKA I guess I will accept because you begged me for it. *blushes*",
            true
        ));
        events.Add(new NextLevelEvent());
        StartNextEvent();
        yield break;
    }
    public IEnumerator WarDarkDialogue()
    {
        events.Clear();
        events.Add(new PostBattleDialogueEvent(
            "War: B-BAKA *hits you and you break a rib*. You’re such a charmer KYAAA *breaks your arm*",
            true
        ));
        events.Add(new PostBattleDialogueEvent(
            "I’m very fragile please be gentler!",
            false
        ));
        events.Add(new PostBattleDialogueEvent(
            "War: W-what are you saying? It’s not like I think you’re so big and strong and would be able to handle me or anything! BAKA *slaps your face so hard your eye swells up so you can’t see out of it*",
            true
        ));
        events.Add(new NextLevelEvent());
        StartNextEvent();
        yield break;
    }

    public IEnumerator FamineLightDialogue()
    {
        events.Clear();
        events.Add(new PostBattleDialogueEvent(
            "Famine: I like you a lot… I see now that love is more than consumption. I shall grant you all the food you will ever need, so that you never hunger for more.",
            true
        ));
        events.Add(new PostBattleDialogueEvent(
            "I appreciate the offer, but what is more precious to me than food is warmth in connection.",
            false
        ));
        events.Add(new PostBattleDialogueEvent(
            "Famine: Then I will stay by your side and quench your thirst.",
            true
        ));
        events.Add(new NextLevelEvent());
        StartNextEvent();
        yield break;
    }

    public IEnumerator FamineDarkDialogue()
    {
        events.Clear();
        events.Add(new PostBattleDialogueEvent(
            "Famine: I like you a lot, little human! I want you to be a part of me… forever! *takes a bite out of your arm*",
            true
        ));
        events.Add(new PostBattleDialogueEvent(
            "ARGHHH AHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH",
            false
        ));
        events.Add(new PostBattleDialogueEvent(
            "Famine: *licks his lips* Don’t worry, I’ll make sure to savor you slowly. You can go talk to my last brother, but don’t keep me waiting for long!",
            true
        ));
        events.Add(new NextLevelEvent());
        StartNextEvent();
        yield break;
    }

    public IEnumerator DeathLightDialogue()
    {
        events.Clear();
        events.Add(new PostBattleDialogueEvent(
            "Death: I am touched by the love you have for humanity. I shall grant you the gift of immortality and youth, so that you never age nor die.",
            true
        ));
        events.Add(new PostBattleDialogueEvent(
            "What makes humanity so precious is how we choose to spend our limited time. I would like to spend it with you.",
            false
        )); 
        events.Add(new PostBattleDialogueEvent(
            "Death: Very well, then I will stay on earth to live with you as you grow old, and lay you to rest when your final day comes.",
            true
        )); 
        events.Add(new NextLevelEvent());
        StartNextEvent();
        yield break;
    }

    public IEnumerator DeathDarkDialogue()
    {
        events.Clear();
        events.Add(new PostBattleDialogueEvent(
            "Death: I want you to stay with me forever. I will kill every other human on earth, everyone you have ever loved, so that you will have no choice but to love me! You will never escape me <3",
            true
        ));
        events.Add(new PostBattleDialogueEvent(
            "No… please no!",
            false
        ));
        events.Add(new PostBattleDialogueEvent(
            "Death: You can only think of me. I will kill my brothers if you even spare them a glance. You will be mine for all eternity!",
            true
        ));
        events.Add(new PostBattleDialogueEvent(
            "...",
            false
        ));
        events.Add(new NextLevelEvent());
        StartNextEvent();
        yield break;
    }
    public IEnumerator FadeToBlack(float duration)
    {
        blackOverlay.gameObject.SetActive(true);

        Color color = blackOverlay.color;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Clamp01(t / duration);
            blackOverlay.color = color;
            yield return null;
        }
    }

    public IEnumerator FadeFromBlack(float duration)
    {
        Color color = blackOverlay.color;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            color.a = 1f - Mathf.Clamp01(t / duration);
            blackOverlay.color = color;
            yield return null;
        }

        blackOverlay.gameObject.SetActive(false);
    }
}