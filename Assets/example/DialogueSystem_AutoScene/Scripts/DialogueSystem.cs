using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DialogueSystem : MonoBehaviour
{
    [System.Serializable]
    public class DialogueEntry
    {
        public string text;
        public Sprite image;
    }

    public GameObject textPrefab;
    public GameObject imagePrefab;
    public Transform contentParent;
    public GameObject nextButton;

    private Queue<DialogueEntry> entriesQueue = new Queue<DialogueEntry>();
    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    public void StartDialogue(List<DialogueEntry> dialogueEntries)
    {
        entriesQueue.Clear();
        foreach (var entry in dialogueEntries)
        {
            entriesQueue.Enqueue(entry);
        }

        ShowNextEntry();
        if (nextButton != null)
            nextButton.SetActive(true);

        if (playerMovement != null)
            playerMovement.canMove = false;
    }

    public void ShowNextEntry()
    {
        if (entriesQueue.Count == 0)
        {
            if (nextButton != null)
                nextButton.SetActive(false);

            if (playerMovement != null)
                playerMovement.canMove = true;

            return;
        }

        var entry = entriesQueue.Dequeue();

        if (!string.IsNullOrEmpty(entry.text))
        {
            GameObject textObj = Instantiate(textPrefab, contentParent);
            textObj.GetComponent<TMP_Text>().text = entry.text;
        }

        if (entry.image != null)
        {
            GameObject imgObj = Instantiate(imagePrefab, contentParent);
            imgObj.GetComponent<Image>().sprite = entry.image;
        }
    }
}