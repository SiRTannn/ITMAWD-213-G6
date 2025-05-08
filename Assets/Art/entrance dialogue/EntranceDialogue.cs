using System.Collections;
using UnityEngine;
using TMPro;

public class EntranceDialogue : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public string[] dialogue;
    public float wordSpeed = 0.05f;

    private int index = 0;
    private bool isDialogueActive = false;
    private bool hasPlayed = false;
    private bool canPressNext = false;

    void Update()
    {
        // Allow next line only when dialogue is active and the player presses 'E'
        if (isDialogueActive && Input.GetKeyDown(KeyCode.E) && canPressNext)
        {
            NextLine();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Start dialogue when the player enters the trigger area and hasn't already triggered the dialogue
        if (other.CompareTag("Player") && !hasPlayed)
        {
            hasPlayed = true;
            dialoguePanel.SetActive(true);
            isDialogueActive = true;
            Time.timeScale = 0; // Freeze everything except dialogue

            StartCoroutine(Typing());
        }
    }

    IEnumerator Typing()
    {
        canPressNext = false;  // Prevent pressing next line until current line is fully typed
        dialogueText.text = ""; // Clear the text before typing the next one
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter; // Add one letter at a time to the text box
            yield return new WaitForSecondsRealtime(wordSpeed); // Use WaitForSecondsRealtime to bypass time freeze
        }
        canPressNext = true; // Allow pressing next once the line is done typing
    }

    public void NextLine()
    {
        // Move to the next line of dialogue
        if (index < dialogue.Length - 1)
        {
            index++;
            StartCoroutine(Typing());
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        // End the dialogue and resume the game
        dialogueText.text = "";
        index = 0;
        isDialogueActive = false;
        canPressNext = false;
        dialoguePanel.SetActive(false);
        Time.timeScale = 1; // Unfreeze the game
    }
}
