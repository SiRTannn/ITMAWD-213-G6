using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public List<DialoguePanel> panels;

    public Button continueButton;
    public Button quizButton;

    public float wordSpeed = 0.02f;
    public bool playerIsClose;

    private int currentPanelIndex = 0;
    private int currentTextIndex = 0;
    private bool isTyping = false;
    private bool lineFullyRevealed = false;

    private Coroutine typingCoroutine;
    private TMP_Text currentlyTypingText;
    private string fullCurrentText;

    [Header("Optional: Disable Player Controls")]
    public MonoBehaviour playerAttackScript;
    public AudioSource playerAudioSource;

    public MathCManager mathCManager;
    public NPCSceneReference nextScene;

    void Start()
    {
        continueButton.onClick.AddListener(HandleNext);

        if (quizButton != null)
        {
            quizButton.gameObject.SetActive(false);
            quizButton.onClick.AddListener(LoadQuizScene);
        }

        dialoguePanel.SetActive(false);

        foreach (var panel in panels)
        {
            panel.panelObject.SetActive(false);
            foreach (var text in panel.dialogueTexts)
            {
                text.gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose && mathCManager.AllItemsCollected())
        {
            if (!dialoguePanel.activeInHierarchy)
            {
                dialoguePanel.SetActive(true);
                ShowPanel(currentPanelIndex);
                DisablePlayerControl(true);
            }
        }
    }

    void ShowPanel(int index)
    {
        if (index >= panels.Count)
        {
            EndDialogue();
            return;
        }

        foreach (var panel in panels)
        {
            panel.panelObject.SetActive(false);
            foreach (var text in panel.dialogueTexts)
            {
                if (text != null)
                    text.gameObject.SetActive(false);
            }

            if (panel.lessonTitle != null)
                panel.lessonTitle.gameObject.SetActive(false);
        }

        DialoguePanel currentPanel = panels[index];

        currentPanel.panelObject.SetActive(true);

        if (currentPanel.lessonTitle != null)
        {
            currentPanel.lessonTitle.gameObject.SetActive(true);
        }

        currentTextIndex = 0;
        ShowTextLine(index, 0);
    }

    void ShowTextLine(int panelIndex, int textIndex)
    {
        if (textIndex >= panels[panelIndex].dialogueTexts.Length)
        {
            // Don't call ShowPanel() here anymore
            return;
        }

        TMP_Text currentText = panels[panelIndex].dialogueTexts[textIndex];
        currentText.gameObject.SetActive(true);
        currentlyTypingText = currentText;
        fullCurrentText = currentText.text;
        typingCoroutine = StartCoroutine(TypeText(currentText));
    }

    IEnumerator TypeText(TMP_Text textComponent)
    {
        isTyping = true;
        lineFullyRevealed = false;
        textComponent.text = "";

        foreach (char c in fullCurrentText)
        {
            textComponent.text += c;
            yield return new WaitForSecondsRealtime(wordSpeed);
        }

        isTyping = false;
        lineFullyRevealed = true;
    }

    public void HandleNext()
    {
        if (!lineFullyRevealed)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            currentlyTypingText.text = fullCurrentText;
            isTyping = false;
            lineFullyRevealed = true;
            return;
        }

        currentTextIndex++;
        if (currentPanelIndex < panels.Count &&
            currentTextIndex < panels[currentPanelIndex].dialogueTexts.Length)
        {
            ShowTextLine(currentPanelIndex, currentTextIndex);
        }
        else
        {
            currentPanelIndex++;
            ShowPanel(currentPanelIndex);
        }

        if (currentPanelIndex >= panels.Count)
        {
            continueButton.interactable = false;
            if (quizButton != null)
                quizButton.gameObject.SetActive(true);
        }
    }

    void EndDialogue()
    {
        foreach (var panel in panels)
        {
            panel.panelObject.SetActive(false);
        }

        dialoguePanel.SetActive(false);
        currentPanelIndex = 0;
        currentTextIndex = 0;
        isTyping = false;
        continueButton.interactable = true;

        if (quizButton != null)
            quizButton.gameObject.SetActive(false);

        Time.timeScale = 1;
        DisablePlayerControl(false);
    }

    void LoadQuizScene()
    {
        if (nextScene != null && !string.IsNullOrEmpty(nextScene.ScenePath))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene.ScenePath);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && mathCManager.AllItemsCollected())
            playerIsClose = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            EndDialogue();
        }
    }

    void DisablePlayerControl(bool disabled)
    {
        if (playerAttackScript != null)
            playerAttackScript.enabled = !disabled;

        if (playerAudioSource != null)
            playerAudioSource.mute = disabled;
    }
}
