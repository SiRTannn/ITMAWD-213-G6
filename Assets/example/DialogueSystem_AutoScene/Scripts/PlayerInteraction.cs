using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    private DialogueTrigger currentTrigger;
    public GameObject interactText;

    void Update()
    {
        if (currentTrigger != null && Input.GetKeyDown(KeyCode.E))
        {
            currentTrigger.TriggerDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        currentTrigger = other.GetComponent<DialogueTrigger>();
        if (currentTrigger != null && interactText != null)
        {
            interactText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (currentTrigger == other.GetComponent<DialogueTrigger>())
        {
            currentTrigger = null;
            if (interactText != null)
            {
                interactText.SetActive(false);
            }
        }
    }
}