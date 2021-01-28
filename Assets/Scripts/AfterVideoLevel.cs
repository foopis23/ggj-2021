using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class AfterVideoLevel : MonoBehaviour
{
    public VideoPlayer video;
    public int nextSceneIndex;

    // Start is called before the first frame update
    void Start()
    {
        video.loopPointReached += VideoEnd;
    }

    void VideoEnd(VideoPlayer vp) {
        SceneManager.LoadScene(nextSceneIndex);
    }
}
