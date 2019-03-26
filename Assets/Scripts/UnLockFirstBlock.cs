using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnLockFirstBlock : MonoBehaviour
{
    [SerializeField] private DialogueTrigger dialogueTrigger;

    [SerializeField] private GameObject firstBlock;
    // Start is called before the first frame update
    void Start()
    {
        dialogueTrigger.OnDialogueFinishCallback += Unlock;
    }

    public void Unlock() {
        if (firstBlock != null)
        {
            firstBlock.SetActive(false);            
        }
    }
}
