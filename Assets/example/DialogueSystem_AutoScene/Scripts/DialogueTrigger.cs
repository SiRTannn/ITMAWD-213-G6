using UnityEngine;
using System.Collections.Generic;

public class DialogueTrigger : MonoBehaviour
{
    public List<DialogueSystem.DialogueEntry> dialogueEntries;

    public void TriggerDialogue()
    {
        DialogueSystem dialogueSystem = FindObjectOfType<DialogueSystem>();
        if (dialogueSystem != null)
        {
            dialogueSystem.StartDialogue(dialogueEntries);
        }
    }
}