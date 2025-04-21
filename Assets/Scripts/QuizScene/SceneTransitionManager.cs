using UnityEngine;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    public Vector3 savedPlayerPosition;
    public bool hasReadDialogue;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayerState(Vector3 playerPosition, bool dialogueRead)
    {
        savedPlayerPosition = playerPosition;
        hasReadDialogue = dialogueRead;
    }

    public void RestorePlayerState(GameObject player)
    {
        if (player != null)
        {
            player.transform.position = savedPlayerPosition;
        }
    }
}
