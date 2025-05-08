using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneLoader : MonoBehaviour
{
    // This will hold the reference to the scene
    public SceneAsset sceneAsset;

    // Method to load the scene
    public void LoadQuizScene()
    {
        if (sceneAsset != null)
        {
            // Get the scene name from the SceneAsset
            string sceneName = sceneAsset.name;
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("SceneAsset not assigned!");
        }
    }
}
