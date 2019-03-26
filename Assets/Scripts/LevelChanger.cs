using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    private static readonly int FadeOut = Animator.StringToHash("FadeOut");


    public Animator Animator;

    private int levelToLoad;
    public static LevelChanger Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null) Instance = this;
    }


    public void FadeToLevel(int levelIndex)
    {
        levelToLoad = levelIndex;
        Animator.SetTrigger(FadeOut);
    }

    public void FadeToNextLevel()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}