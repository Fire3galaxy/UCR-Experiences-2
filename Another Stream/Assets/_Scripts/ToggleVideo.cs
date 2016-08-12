using UnityEngine;
using System.Collections;
using System;

public class ToggleVideo : MonoBehaviour, IGvrGazeResponder {
    public PlayVideos videoInterface;

    public void OnGazeEnter()
    {
        return;
    }

    public void OnGazeExit()
    {
        return;
    }

    public void OnGazeTrigger()
    {
        //videoInterface.ToNextVideo();
    }
}
