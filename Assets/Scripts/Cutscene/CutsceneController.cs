using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CutsceneController : MonoBehaviour
{
    public CutscenePanel[] panels;
    public Button nextButton;
    public float typingSpeed = 0.05f;
    public float autoProgressDelay = 3.0f;
    public bool useAutoProgress = true;
    public SceneReference nextScene;
    public AudioClip typingSFX;
    public AudioSource audioSource;

    private int currentPanelIndex = 0;
    private int currentDialogueIndex = 0;
    private bool isTyping = false;
    private Coroutine autoProgressCoroutine;

    [System.Serializable]
    public class CutscenePanel
    {
        public GameObject backgroundImage;
        public TMP_Text[] dialogueTexts;
    }

    void Start()
    {
        nextButton.onClick.AddListener(HandleNextButtonClick);

        foreach (var panel in panels)
        {
            panel.backgroundImage.SetActive(false);
            foreach (var dialogueText in panel.dialogueTexts)
            {
                dialogueText.gameObject.SetActive(false);
            }
        }

        ShowPanel(currentPanelIndex);
    }

    void ShowPanel(int index)
    {
        if (index >= panels.Length)
        {
            Debug.Log("Cutscene completed.");
            LoadNextScene();
            return;
        }

        foreach (var panel in panels)
        {
            panel.backgroundImage.SetActive(false);
            foreach (var dialogueText in panel.dialogueTexts)
            {
                dialogueText.gameObject.SetActive(false);
            }
        }

        panels[index].backgroundImage.SetActive(true);
        currentDialogueIndex = 0;
        ShowDialogue(currentPanelIndex, currentDialogueIndex);
    }

    void ShowDialogue(int panelIndex, int dialogueIndex)
    {
        if (dialogueIndex >= panels[panelIndex].dialogueTexts.Length)
        {
            currentPanelIndex++;
            ShowPanel(currentPanelIndex);
            return;
        }

        TMP_Text currentText = panels[panelIndex].dialogueTexts[dialogueIndex];
        currentText.gameObject.SetActive(true);

        string fullText = currentText.text;
        currentText.text = "";
        StartCoroutine(TypeText(currentText, fullText));
    }

    IEnumerator TypeText(TMP_Text textComponent, string fullText)
    {
        isTyping = true;
        textComponent.text = "";

        StartTypingLoop();

        foreach (char letter in fullText)
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        StopTypingLoop();

        isTyping = false;

        if (useAutoProgress)
        {
            if (autoProgressCoroutine != null)
            {
                StopCoroutine(autoProgressCoroutine);
            }

            autoProgressCoroutine = StartCoroutine(AutoProgressDialogue());
        }
    }

    void StartTypingLoop()
    {
        if (audioSource != null && typingSFX != null)
        {
            audioSource.clip = typingSFX;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void StopTypingLoop()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.loop = false;
        }
    }

    IEnumerator AutoProgressDialogue()
    {
        yield return new WaitForSeconds(autoProgressDelay);
        HandleNextButtonClick();
    }

    public void HandleNextButtonClick()
    {
        if (autoProgressCoroutine != null)
        {
            StopCoroutine(autoProgressCoroutine);
            autoProgressCoroutine = null;
        }

        if (isTyping)
        {
            StopAllCoroutines();
            StopTypingLoop();

            TMP_Text currentText = panels[currentPanelIndex].dialogueTexts[currentDialogueIndex];
            currentText.text = currentText.GetComponent<TMP_Text>().text;
            isTyping = false;

            if (useAutoProgress)
            {
                autoProgressCoroutine = StartCoroutine(AutoProgressDialogue());
            }
        }
        else
        {
            currentDialogueIndex++;
            ShowDialogue(currentPanelIndex, currentDialogueIndex);
        }
    }

    void LoadNextScene()
    {
        if (nextScene != null && !string.IsNullOrEmpty(nextScene.ScenePath))
        {
            SceneManager.LoadScene(nextScene.ScenePath);
        }
        else
        {
            Debug.LogWarning("Next scene not set! Please drag and drop a scene in the Inspector.");
        }
    }
}
