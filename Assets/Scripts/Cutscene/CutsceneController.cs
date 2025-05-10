using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CutsceneController : MonoBehaviour
{
    [Header("Cutscene Settings")]
    public CutscenePanel[] panels;  // Array to store the cutscene panels
    public Button nextButton;      // Button to go to the next dialogue or skip text
    public float typingSpeed = 0.05f;  // Typing speed (lower is faster)
    public float autoProgressDelay = 3.0f;  // Time to wait before automatically progressing (in seconds)
    public bool useAutoProgress = true;  // Toggle for auto-progression feature

    [Header("Next Scene Settings")]
    public SceneReference nextScene;  // Drag and drop scene reference

    private int currentPanelIndex = 0;  // Index of the current panel
    private int currentDialogueIndex = 0;  // Index of the current dialogue text
    private bool isTyping = false;  // Flag to control if text is currently being typed
    private Coroutine autoProgressCoroutine;  // Reference to the auto-progress coroutine

    [System.Serializable]
    public class CutscenePanel
    {
        public GameObject backgroundImage;  // The background image/panel
        public TMP_Text[] dialogueTexts;    // Array of dialogue texts for this panel
    }

    void Start()
    {
        nextButton.onClick.AddListener(HandleNextButtonClick);

        // Hide all dialogues initially
        foreach (var panel in panels)
        {
            panel.backgroundImage.SetActive(false);
            foreach (var dialogueText in panel.dialogueTexts)
            {
                dialogueText.gameObject.SetActive(false);
            }
        }

        // Start the cutscene
        ShowPanel(currentPanelIndex);
    }

    // Method to display the current panel (image and dialogue text)
    void ShowPanel(int index)
    {
        if (index >= panels.Length)
        {
            Debug.Log("Cutscene completed.");
            LoadNextScene();  // Load next scene after cutscene is done
            return;  // End the cutscene if no more panels are left
        }

        // Deactivate all panels first
        foreach (var panel in panels)
        {
            panel.backgroundImage.SetActive(false);

            // Hide all dialogue texts from previous panels
            foreach (var dialogueText in panel.dialogueTexts)
            {
                dialogueText.gameObject.SetActive(false);
            }
        }

        // Activate the background image for the current panel
        panels[index].backgroundImage.SetActive(true);

        // Reset dialogue index and show the first dialogue for this panel
        currentDialogueIndex = 0;
        ShowDialogue(currentPanelIndex, currentDialogueIndex);
    }

    // Method to show a specific dialogue in a panel
    void ShowDialogue(int panelIndex, int dialogueIndex)
    {
        if (dialogueIndex >= panels[panelIndex].dialogueTexts.Length)
        {
            // Move to the next panel if no more dialogues
            currentPanelIndex++;
            ShowPanel(currentPanelIndex);
            return;
        }

        // Show the current dialogue without hiding previous ones
        TMP_Text currentText = panels[panelIndex].dialogueTexts[dialogueIndex];
        currentText.gameObject.SetActive(true);

        // Store the full text to display
        string fullText = currentText.text;

        // Clear the text and start typing
        currentText.text = "";
        StartCoroutine(TypeText(currentText, fullText));
    }

    // Coroutine to type out the text letter by letter
    IEnumerator TypeText(TMP_Text textComponent, string fullText)
    {
        isTyping = true;
        textComponent.text = "";  // Clear text

        // Type each character with a delay
        foreach (char letter in fullText)
        {
            textComponent.text += letter;  // Add one character at a time
            yield return new WaitForSeconds(typingSpeed);  // Wait before showing next letter
        }

        isTyping = false;  // Typing is finished

        // Start auto-progress if enabled
        if (useAutoProgress)
        {
            // Cancel any existing auto-progress coroutines
            if (autoProgressCoroutine != null)
            {
                StopCoroutine(autoProgressCoroutine);
            }

            // Start new auto-progress coroutine
            autoProgressCoroutine = StartCoroutine(AutoProgressDialogue());
        }
    }

    // Coroutine to automatically progress to the next dialogue after a delay
    IEnumerator AutoProgressDialogue()
    {
        yield return new WaitForSeconds(autoProgressDelay);
        HandleNextButtonClick();
    }

    // Method called when the Next button is clicked
    void HandleNextButtonClick()
    {
        // Cancel any auto-progress coroutine when user clicks manually
        if (autoProgressCoroutine != null)
        {
            StopCoroutine(autoProgressCoroutine);
            autoProgressCoroutine = null;
        }

        if (isTyping)
        {
            // If text is currently typing, skip to show all text immediately
            StopAllCoroutines();
            TMP_Text currentText = panels[currentPanelIndex].dialogueTexts[currentDialogueIndex];
            currentText.text = currentText.GetComponent<TMP_Text>().text;
            isTyping = false;

            // Restart auto-progress after skipping if enabled
            if (useAutoProgress)
            {
                autoProgressCoroutine = StartCoroutine(AutoProgressDialogue());
            }
        }
        else
        {
            // Move to the next dialogue
            currentDialogueIndex++;
            ShowDialogue(currentPanelIndex, currentDialogueIndex);
        }
    }

    // Method to load the next scene after the cutscene
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