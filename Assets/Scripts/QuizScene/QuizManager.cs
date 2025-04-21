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

    private void Start()
    {
        totalQuestions = QnA.Count;
        GoPanel.SetActive(false);
        Quizpanel.SetActive(true);
        UpdateScore();
        GenerateQuestion();
    }

    void GameOver()
    {
        Quizpanel.SetActive(false);
        GoPanel.SetActive(true);
        ScoreTxt.text = "Final Score: " + score + "/" + totalQuestions;
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
}