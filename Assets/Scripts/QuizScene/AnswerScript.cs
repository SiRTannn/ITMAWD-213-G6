using UnityEngine;

public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    [SerializeField]public QuizManager quizManager;
    public void Answer()
    {
        if (isCorrect)
        {
            Debug.Log("Correct Answer");
            quizManager.correct();
        }
        else
        {
            Debug.Log("Wrong Answer");
            quizManager.wrong();
        }
    }
}
