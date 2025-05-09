using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class CutscenePanel
{
    public GameObject bgImage;                  // Your full panel container (Panel1)
    public List<TMP_Text> dialogueTexts;        // Each dialogue line in the panel
}

public class CutsceneController : MonoBehaviour
{
    public List<CutscenePanel> panels;
    public Button nextButton;
    public float typingSpeed = 0.03f;

    private int currentIndex = 0;
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    void Start()
    {
        // Deactivate all first
        foreach (var panel in panels)
        {
            if (panel.bgImage) panel.bgImage.SetActive(false);
        }

        // Show first
        if (panels.Count > 0)
        {
            currentIndex = 0;
            ShowPanel(panels[currentIndex]);
        }

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(OnNextClicked);
    }

    void OnNextClicked()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            ShowFullText(panels[currentIndex].dialogueTexts);
            isTyping = false;
            return;
        }

        panels[currentIndex].bgImage.SetActive(false);
        currentIndex++;

        if (currentIndex < panels.Count)
        {
            ShowPanel(panels[currentIndex]);
        }
        else
        {
            nextButton.gameObject.SetActive(false);
        }
    }

    void ShowPanel(CutscenePanel panel)
    {
        panel.bgImage.SetActive(true);
        foreach (var text in panel.dialogueTexts)
        {
            text.gameObject.SetActive(true);
            text.text = "";
        }

        typingCoroutine = StartCoroutine(TypeTextLines(panel.dialogueTexts));
    }

    IEnumerator TypeTextLines(List<TMP_Text> texts)
    {
        isTyping = true;

        foreach (var text in texts)
        {
            string full = text.GetParsedText();
            text.text = "";

            foreach (char c in full)
            {
                text.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }

            yield return new WaitForSeconds(0.3f);
        }

        isTyping = false;
    }

    void ShowFullText(List<TMP_Text> texts)
    {
        foreach (var text in texts)
        {
            text.text = text.GetParsedText();
        }
    }
}
