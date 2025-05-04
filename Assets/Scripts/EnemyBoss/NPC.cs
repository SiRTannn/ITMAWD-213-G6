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
    public float wordSpeed = 0.02f;
    public bool playerIsClose;

    private int index = 0;
    private bool isTyping = false;

    [Header("Optional: Disable Player Controls")]
    public MonoBehaviour playerAttackScript;  // Drag your PlayerAttack script here
    public AudioSource playerAudioSource;     // Drag your AudioSource here

    void Start()
    {
        continueButton.onClick.AddListener(ShowNext);
        dialoguePanel.SetActive(false);

        foreach (var obj in dialogueObjects)
        {
            obj.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose)
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
        if (other.CompareTag("Player"))
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
    }

    void DisablePlayerControl(bool disabled)
    {
        if (playerAttackScript != null)
            playerAttackScript.enabled = !disabled;

        if (playerAudioSource != null)
            playerAudioSource.mute = disabled;
    }
}
