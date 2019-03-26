using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUi : MonoBehaviour
{
    public Image hpBar;
    public Image mpBar;
    public static GameUi Instance { get; private set; }

    private void Start()
    {
//        hpBar.fillAmount = 1;
//        mpBar.fillAmount = 0;
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
}
