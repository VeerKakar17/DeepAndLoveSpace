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

    public DialogueEvent(string dialogueText) : base()
    {
        this.dialogueText = dialogueText;
    }

    public override void StartEvent()
    {
        Debug.Log("Dialogue: " + dialogueText);
    }

    public override void UpdateEvent()
    {
        // todo - type writer effect

        // detect input (skip dialogue)
        foreach (var key in Keyboard.current.allKeys)
        {
            if (key.wasPressedThisFrame)
            {
                EndEvent();
                break;
            }
        }
    }
    
    public override void EndEvent()
    {
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