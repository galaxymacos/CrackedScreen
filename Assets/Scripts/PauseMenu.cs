using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private int menuIndex = 0;

    public GameObject PauseMenuUI;
    public GameObject InGameUi;
    public GameObject DialogueWindow;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

//    private void Start()
//    {
//        InGameUi.SetActive(true);
//        DialogueWindow.SetActive(true);
//        print("Set ui to true");
//    }
//
//    private void OnEnable()
//    {
//        InGameUi.SetActive(false);
//        DialogueWindow.SetActive(false);
//        print("Set ui to false");
//    }
//
//    private void OnDisable()
//    {
//        InGameUi.SetActive(true);
//        DialogueWindow.SetActive(true);
//        print("Set ui to true");
//    }

    private void Pause()
    {
        PauseMenuUI.SetActive(true);
        GameManager.Instance.gameIsPaused = true;

        // No need to change timeScale if dialogue window is opened (which means timescale is already 0)
        if (!DialogueManager.Instance._dialogueStarted)    
        {
            Time.timeScale = 0f;            
        }
    }
    

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        GameManager.Instance.gameIsPaused = false;
        
        // No need to change timeScale if dialogue window is opened (which means timescale is already 0)
        if (!DialogueManager.Instance._dialogueStarted)
        {
            Time.timeScale = 1f;            
        }

    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}