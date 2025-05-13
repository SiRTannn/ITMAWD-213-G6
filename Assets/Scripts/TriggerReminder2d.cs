using UnityEngine;
using TMPro;
using System.Collections;

public class TriggerReminder2D : MonoBehaviour
{
    public TMP_Text dialogueText;         
    public string[] messages;             
    public float typingSpeed = 0.04f;     

    private bool hasTriggered = false;
    private bool isTyping = false;
    private int currentLine = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasTriggered && collision.CompareTag("Player"))
        {
            hasTriggered = true;
            dialogueText.gameObject.SetActive(true);
            StartCoroutine(TypeText(messages[currentLine]));
        }
    }

    void Update()
    {
        if (hasTriggered && !isTyping && Input.GetKeyDown(KeyCode.E))
        {
            currentLine++;
            if (currentLine < messages.Length)
            {
                StartCoroutine(TypeText(messages[currentLine]));
            }
            else
            {
                dialogueText.gameObject.SetActive(false);
                Destroy(gameObject); 
            }
        }
    }

    IEnumerator TypeText(string message)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in message)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}
