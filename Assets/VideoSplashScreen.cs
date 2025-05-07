using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoSplashScreen : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName = "Scene 1";

    void Start()
    {
        videoPlayer.Play();
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
