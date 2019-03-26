using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsTriggerByCollision : MonoBehaviour
{
    public string tip;

    private bool PanelIsOpen;
    private bool PanelAbandoned;


    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.player)
        {
            Interact();
        }
    }

    private void Interact()
    {
        if (!PanelIsOpen)
        {
            PanelIsOpen = true;
            TipsManager.Instance.PanelStartTime = Time.time;
            TipsManager.Instance.tipsPanelText.text = tip;
            TipsManager.Instance.TipsPanelAnimator.SetBool("isOpen", true);
        }
    }

    private void Update()
    {
        if (PanelIsOpen && !PanelAbandoned && Time.time - TipsManager.Instance.PanelStartTime > TipsManager.Instance.tipsDisplayDuration)
        {
            TipsManager.Instance.TipsPanelAnimator.SetBool("isOpen", false);
            PanelAbandoned = true;
        }
    }
}