using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsManager : MonoBehaviour
{
    public static TipsManager Instance { get; private set; }

    public Animator TipsPanelAnimator;
    public Text tipsPanelText;
    public float tipsDisplayDuration = 4f;
    public float PanelStartTime;



    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
}