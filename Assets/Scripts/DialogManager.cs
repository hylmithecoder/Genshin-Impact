using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Image pfDialogueBox;
    public RectTransform dialogueTransform;
    public float typeSpeed = 10f;

    public static DialogueManager Instance { get; private set; }
    private Dialog currentDialogue;
    private bool isDialogueBoxActive;
    public event Action OnDialogueEnd;
    public event Action OnDialogueStart;

    private int currentLine;
    private TextMeshProUGUI dialogueText;
    private TextMeshProUGUI nameText;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartDialogue(Dialog dialogue)
    {
        StartCoroutine(ShowDialogue(dialogue));
    }

    private void InstantiateDialogueBox()
    {
        if (!isDialogueBoxActive)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            dialogueTransform = Instantiate(pfDialogueBox.gameObject, canvas.transform).GetComponent<RectTransform>();
            dialogueText = dialogueTransform.Find("MessageText").GetComponent<TextMeshProUGUI>();
            Image nameBox = dialogueTransform.Find("Name").GetComponent<Image>();
            nameText = nameBox.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
            isDialogueBoxActive = true;
        }
    }

    private IEnumerator ShowDialogue(Dialog dialog)
    {
        InstantiateDialogueBox();
        OnDialogueStart?.Invoke();
        currentDialogue = dialog;
        currentLine = 0;
        nameText.text = dialog.name;
        StartCoroutine(TypeSentenceWithDelay(dialog.DialogueLine[currentLine]));
        yield return null;
    }

    public void EndDialogue()
    {
        MovementHandler.Instance.CanMoving = true;
        dialogueTransform.gameObject.SetActive(false);        
        OnDialogueEnd?.Invoke();
        isDialogueBoxActive = false;
        Destroy(dialogueTransform.gameObject);
    }

    public void DisplayNextSentence()
    {
        if (currentLine < currentDialogue.DialogueLine.Count - 1)
        {
            currentLine++;
            StartCoroutine(TypeSentenceWithDelay(currentDialogue.DialogueLine[currentLine]));
        }
        else
        {
            EndDialogue();
        }
    }

    private IEnumerator TypeSentenceWithDelay(string sentence)
    {
        dialogueText.text = "";

        foreach (char letter in sentence)
        {
            dialogueText.text += letter;
            // if (Input.GetKeyDown(KeyCode.Space))
            // {
            //     dialogueText.text = "";
            //     dialogueText.text = sentence;
            //     Debug.Log("Skip Dialog");
            // }
            yield return new WaitForSeconds(1 / typeSpeed);
        }
        
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        #if UNITY_ANDROID
            yield return new WaitForSeconds(1f);
        #endif
        DisplayNextSentence();
    }
}
