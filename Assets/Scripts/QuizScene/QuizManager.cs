using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public List<QuestionAndAnswers> QnA;
    public GameObject[] option;
    public int currentQuestion;

    public GameObject Quizpanel;
    public GameObject GoPanel;

    public TextMeshProUGUI QuestionTxt;
    public TextMeshProUGUI ScoreTxt;

    private int totalQuestions;
    public int score;

    public Button goBackButton; // Button to go back (displayed when the player fails)
    public Button proceedButton; // Button to proceed (displayed when the player passes)

    // New fields to hold the scene names
    public string nextLevelScene; // Scene name for the next level
    public string previousLevelScene; // Scene name for going back

    private void Start()
    {
        totalQuestions = QnA.Count;
        GoPanel.SetActive(false);
        Quizpanel.SetActive(true);
        goBackButton.gameObject.SetActive(false); // Initially hide the "Go Back" button
        proceedButton.gameObject.SetActive(false); // Initially hide the "Proceed" button
        UpdateScore();
        GenerateQuestion();
    }

    void GameOver()
    {
        Quizpanel.SetActive(false);
        GoPanel.SetActive(true);
        ScoreTxt.text = "Final Score: " + score + "/" + totalQuestions;

        // Calculate the percentage score
        float percentage = (float)score / totalQuestions * 100;

        if (percentage >= 70) // If the score is 70% or higher, show proceed button
        {
            proceedButton.gameObject.SetActive(true);
            proceedButton.onClick.AddListener(ProceedToNextLevel); // Attach the scene transition
        }
        else // If the score is below 70%, show go back button
        {
            goBackButton.gameObject.SetActive(true);
            goBackButton.onClick.AddListener(GoBackToPreviousScene); // Attach the scene transition
        }
    }

    public void correct()
    {
        score += 1;
        QnA.RemoveAt(currentQuestion);
        CheckAndGenerateNextQuestion();
    }

    public void wrong()
    {
        QnA.RemoveAt(currentQuestion);
        CheckAndGenerateNextQuestion();
    }

    void CheckAndGenerateNextQuestion()
    {
        if (QnA.Count > 0)
        {
            GenerateQuestion();
        }
        else
        {
            Debug.Log("Out of Questions");
            GameOver();
        }
        UpdateScore();
    }

    void SetAnswer()
    {
        for (int i = 0; i < option.Length; i++)
        {
            option[i].GetComponent<AnswerScript>().isCorrect = false;

            option[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = QnA[currentQuestion].Answer[i];

            if (QnA[currentQuestion].CorrectAnswer == i + 1)
            {
                option[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }

    void GenerateQuestion()
    {
        currentQuestion = Random.Range(0, QnA.Count);
        QuestionTxt.text = QnA[currentQuestion].Question;
        SetAnswer();
    }

    void UpdateScore()
    {
        ScoreTxt.text = "Score: " + score + "/" + totalQuestions;
    }

    // Proceed to the next level (you can implement scene loading here)
    public void ProceedToNextLevel()
    {
        if (!string.IsNullOrEmpty(nextLevelScene))
        {
            SceneManager.LoadScene(nextLevelScene); // Load the next level scene
            Debug.Log("Proceeding to next level...");
        }
        else
        {
            Debug.LogError("Next level scene is not assigned!");
        }
    }

    // Go back to the previous scene
    public void GoBackToPreviousScene()
    {
        if (!string.IsNullOrEmpty(previousLevelScene))
        {
            SceneManager.LoadScene(previousLevelScene); // Load the previous level scene
            Debug.Log("Going back to the previous level...");
        }
        else
        {
            Debug.LogError("Previous level scene is not assigned!");
        }
    }
}
