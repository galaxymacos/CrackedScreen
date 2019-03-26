using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Animator animator;
    public Text dialogueText;
    public bool _dialogueStarted;

    public Text nameText;

    public Queue<string> sentences;

    public Dialogue[] dialogues;
    public int currentDialogue = 0;
    private PlayerController playerController;
    public bool currentDialogueHasDisplayed;
    public static DialogueManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        currentDialogueHasDisplayed = true;
        sentences = new Queue<string>();
        playerController = GameManager.Instance.player.GetComponent<PlayerController>();
    }


    public void StartDialogue(Dialogue[] _dialogues)
    {
        currentDialogueHasDisplayed = false;
        playerController.canControl = false;
        AudioManager.instance.StopAllSfx();
        dialogues = _dialogues;
        animator.SetBool("isOpen", true);
        // Pause the game if dialogue is playing
        Time.timeScale = 0f;

        nameText.text = dialogues[currentDialogue].name;

        sentences.Clear();
        foreach (var sentence in dialogues[currentDialogue].sentences) sentences.Enqueue(sentence);
        DisplayNextSentence();
    }

    public bool DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            if (!LoadNextDialogue())
            {
                EndDialogue();
                return false;
            }
        }

        var sentence = sentences.Dequeue();
        StopAllCoroutines();
        AudioManager.instance.PlaySfx("Type");
        StartCoroutine(TypeSentence(sentence));
        return true;
    }

    private bool LoadNextDialogue()
    {
        currentDialogue += 1;
        if (currentDialogue >= dialogues.Length)
        {
            return false;
        }

        nameText.text = dialogues[currentDialogue].name;

        sentences.Clear();
        foreach (var sentence in dialogues[currentDialogue].sentences) sentences.Enqueue(sentence);
        return true;
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (var letter in sentence)
        {
            dialogueText.text += letter;
            if (dialogueText.text == sentence)
            {
                currentDialogueHasDisplayed = true;
                AudioManager.instance.StopSfx("Type");
            }
            yield return null;
        }
    }

    private void EndDialogue()
    {
        playerController.canControl = true;
        Time.timeScale = 1f;
        currentDialogue = 0;
        _dialogueStarted = false;
        animator.SetBool("isOpen", false);
    }
}