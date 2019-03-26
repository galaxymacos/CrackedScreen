using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : Interactable
{
    public Dialogue[] dialogues; // Store the dialogues between NPC and player
    public int currentDialogue = 0;


    public delegate void OnDialogueFinish();

    public OnDialogueFinish OnDialogueFinishCallback;
    
    private PlayerController playerController;


    private void Start()
    {
        playerController = GameManager.Instance.player.GetComponent<PlayerController>();
        
    }

    public override void Interact()
    {
        if (!DialogueManager.Instance.currentDialogueHasDisplayed)
        {
            return;
        }
        hasBeenInteracted = true;
        if (!playerController.canControlDialogueBox)
        {
            return;            
        }
        else
        {
        }
        if (!DialogueManager.Instance._dialogueStarted && currentDialogue < dialogues.Length)
        {
            DialogueManager.Instance.StartDialogue(dialogues);
            DialogueManager.Instance._dialogueStarted = true;
        }
        else
        {
            if (!DialogueManager.Instance.DisplayNextSentence())
            {
                enabled = false;
                if (currentDialogue + 1 < dialogues.Length)
                {
                    currentDialogue += 1;
                    DialogueManager.Instance._dialogueStarted = false;
                }
                else
                {
//                    OnDialogueFinishCallback?.Invoke();
                }
            }
        }
    }

    private void OnDisable() {

        OnDialogueFinishCallback?.Invoke();

    }
}