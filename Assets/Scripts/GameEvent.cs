using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class GameEvent
{

    public GameEvent()
    {

    }

    public virtual void StartEvent()
    {
        // To be overridden by subclasses
    }

    public virtual void UpdateEvent()
    {
        // To be overridden by subclasses
    }

    public virtual void EndEvent()
    {
        BulletSpawner.Instance.ResetBullets();
        GameManager.Instance.StartNextEvent();
    }

}

public class DialogueEvent : GameEvent
{
    public string dialogueText;
    private int currCharacter;
    private float timer;
    private const float TIME_BETWEEN_CHARS = 0.025f;
    private const float DIALOGUE_END_TIME = 3f;

    public DialogueEvent(string dialogueText) : base()
    {
        this.dialogueText = dialogueText;
    }

    public override void StartEvent()
    {
        currCharacter = 0;
        timer = 0;
        GameManager.Instance.StartDialogue();
        Debug.Log("Dialogue: " + dialogueText);
    }

    public override void UpdateEvent()
    {
        timer += Time.deltaTime;
        if (currCharacter < dialogueText.Length && TIME_BETWEEN_CHARS <= timer)
        {
            timer -= TIME_BETWEEN_CHARS;
            currCharacter++;
            GameManager.Instance.SetDialogue(dialogueText.Substring(0, currCharacter));
        }

        if (currCharacter == dialogueText.Length && timer >= DIALOGUE_END_TIME)
        {
            EndEvent();
        }

        // detect input (skip dialogue)
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (currCharacter < dialogueText.Length)
            {
                timer = 0;
                currCharacter = dialogueText.Length;
                GameManager.Instance.SetDialogue(dialogueText);
            } else if (timer > 0.1f)
            {
                EndEvent();
            }
        }
    }
    
    public override void EndEvent()
    {
        GameManager.Instance.ClearDialogue();
        GameManager.Instance.StartNextEvent();
    }

}

public class AttackEvent : GameEvent
{

    public Attack attack;

    public AttackEvent(Attack a) : base()
    {
        attack = a;
    }

    public override void StartEvent()
    {
        Debug.Log("Attack Event Started");
        attack.Init();
    }

    public override void UpdateEvent()
    {
        attack.Update();
    }
}