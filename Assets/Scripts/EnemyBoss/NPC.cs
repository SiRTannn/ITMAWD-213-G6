using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public List<GameObject> dialogueObjects;
    public List<bool> isImageFlags;

    public Button continueButton;
    public Button quizButton; // ✅ NEW: Quiz button reference

    public float wordSpeed = 0.02f;
    public bool playerIsClose;

    private int index = 0;
    private bool isTyping = false;

    [Header("Optional: Disable Player Controls")]
    public MonoBehaviour playerAttackScript;  // Drag your PlayerAttack script here
    public AudioSource playerAudioSource;     // Drag your AudioSource here

    public MathCManager mathCManager;  // Add reference to MathCManager script to track collectables

    void Start()
    {
        continueButton.onClick.AddListener(ShowNext);
        dialoguePanel.SetActive(false);

        foreach (var obj in dialogueObjects)
        {
            obj.SetActive(false);
        }

        if (quizButton != null)
            quizButton.gameObject.SetActive(false); // ✅ Hide quiz button on start
    }

    void Update()
    {
        // Allow dialogue interaction only if player is close and all collectables have been found
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose && mathCManager.AllItemsCollected())
        {
            if (!dialoguePanel.activeInHierarchy)
            {
                dialoguePanel.SetActive(true);
                ShowNext();
                DisablePlayerControl(true);
            }
        }
    }

    void ShowNext()
    {
        if (index >= dialogueObjects.Count || isTyping)
            return;

        GameObject currentObj = dialogueObjects[index];
        bool isImage = isImageFlags[index];

        currentObj.SetActive(true);

        if (!isImage)
        {
            TextMeshProUGUI tmp = currentObj.GetComponent<TextMeshProUGUI>();
            StartCoroutine(TypeText(tmp));
        }

        index++;

        if (index >= dialogueObjects.Count)
        {
            continueButton.interactable = false;

            // ✅ Show quiz button now
            if (quizButton != null)
                quizButton.gameObject.SetActive(true);
        }
    }

    IEnumerator TypeText(TextMeshProUGUI textComponent)
    {
        isTyping = true;

        string fullText = textComponent.text;
        textComponent.text = "";

        foreach (char c in fullText)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(wordSpeed);
        }

        isTyping = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && mathCManager.AllItemsCollected())  // Only allow interaction if items are collected
            playerIsClose = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            ResetDialogue();
            DisablePlayerControl(false);
        }
    }

    void ResetDialogue()
    {
        foreach (var obj in dialogueObjects)
        {
            obj.SetActive(false);
        }

        index = 0;
        isTyping = false;
        continueButton.interactable = true;
        dialoguePanel.SetActive(false);

        // ✅ Also hide quiz button again if needed
        if (quizButton != null)
            quizButton.gameObject.SetActive(false);
    }

    void DisablePlayerControl(bool disabled)
    {
        if (playerAttackScript != null)
            playerAttackScript.enabled = !disabled;

        if (playerAudioSource != null)
            playerAudioSource.mute = disabled;
    }
}
