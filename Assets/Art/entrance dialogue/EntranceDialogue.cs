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
        if (isDialogueActive && Input.GetKeyDown(KeyCode.E) && canPressNext)
        {
            NextLine();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasPlayed)
        {
            hasPlayed = true;
            dialoguePanel.SetActive(true);
            isDialogueActive = true;
            StartCoroutine(Typing());
        }
    }

    IEnumerator Typing()
    {
        canPressNext = false;
        dialogueText.text = "";
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
        canPressNext = true;
    }

    public void NextLine()
    {
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
        dialogueText.text = "";
        index = 0;
        isDialogueActive = false;
        canPressNext = false;
        dialoguePanel.SetActive(false);
    }
}
