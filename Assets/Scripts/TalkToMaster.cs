using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkToMaster : MonoBehaviour {
    private DialogueTrigger blockDialogueTrigger;
    [SerializeField] private DialogueTrigger masterSecondDialogue;


    private void Start() {
        blockDialogueTrigger = GetComponent<DialogueTrigger>();
        blockDialogueTrigger.OnDialogueFinishCallback += EnableMasterDialogue;
    }

    private void EnableMasterDialogue() {
        if (masterSecondDialogue != null)
        {
            masterSecondDialogue.enabled = true;
            
        }

    }    
    
}