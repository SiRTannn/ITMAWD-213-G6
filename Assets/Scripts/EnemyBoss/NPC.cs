using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public string[] dialogue;
    private int index;

    public GameObject contButton;
    public GameObject quizButton; // New quiz button
    public float wordSpeed;
    public bool playerIsClose;

    void Start()
    {
        if (quizButton != null)
            quizButton.SetActive(false); // Ensure quiz button is hidden at start

        dialoguePanel.SetActive(false); // Ensure dialogue panel is hidden at start
        contButton.SetActive(false); // Ensure continue button is hidden
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose)
        {
            if (dialoguePanel.activeInHierarchy)
            {
                zeroText();
            }
            else
            {
                dialoguePanel.SetActive(true);
                if (quizButton != null) quizButton.SetActive(false); // Hide quiz button when starting dialogue
                StartCoroutine(Typing());
            }
        }

        if (dialogueText.text == dialogue[index])
        {
            contButton.SetActive(true);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
        contButton.SetActive(false);
        if (quizButton != null) quizButton.SetActive(false); // Hide quiz button when resetting dialogue
    }

    IEnumerator Typing()
    {
        dialogueText.text = ""; // Ensure text is cleared before starting typing
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {
        contButton.SetActive(false);
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            dialoguePanel.SetActive(false);
            if (quizButton != null) quizButton.SetActive(true); // Show quiz button after last dialogue
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            zeroText(); // Reset everything when player leaves
        }
    }
}
