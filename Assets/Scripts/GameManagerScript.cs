using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUI;

    public void gameOver()
    {
        StartCoroutine(ShowGameOverAfterDelay());
    }

    IEnumerator ShowGameOverAfterDelay()
    {
        yield return new WaitForSeconds(1.5f); // match death animation duration
        gameOverUI.SetActive(true);
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Scene 1");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}