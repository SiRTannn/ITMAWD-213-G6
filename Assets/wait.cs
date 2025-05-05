using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class wait : MonoBehaviour
{

    public float wait_time = 5f; 

    void Start()
    {
        StartCoroutine(wait_for_intro());
    }

    IEnumerator wait_for_intro()
    {
        yield return new WaitForSeconds(wait_time);
        Debug.Log("Waited for 2 seconds");
        
        SceneManager.LoadScene("Scene 1"); 
    }

}
