using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class DialogueController : MonoBehaviour
{
    [Header("Scene Transition")]
    public string sceneToLoad; // The name of the scene to load after dialogue

    [Header("UI Elements")]
    public TextMeshProUGUI textComponent; // The dialogue text component
    public Button transferButton; // Button for transferring scenes
    public string[] lines; // Array of dialogue lines
    public float textSpeed; // Speed of text animation

    private int index;

    void Start()
    {
        textComponent.text = string.Empty;
        transferButton.gameObject.SetActive(false); // Hide the transfer button initially
        transferButton.onClick.AddListener(TransferScene); // Add listener for the transfer button
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        textComponent.text = ""; // Clear the text
        transferButton.gameObject.SetActive(true); // Show the transfer button
    }

    void TransferScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad); // Load the specified scene
        }
        else
        {
            Debug.LogError("Scene to load is not specified!");
        }
    }
}