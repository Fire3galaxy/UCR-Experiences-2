using UnityEngine;
using System.Collections;
using System;

public class WallTrigger : MonoBehaviour, IGvrGazeResponder {
    public PlayVideos VideoController;
    public int direction = -1;

    public void OnGazeEnter()
    {

    }

    public void OnGazeExit()
    {
        
    }

    public void OnGazeTrigger()
    {
        switch (direction)
        {
            case DirectionValues.NORTH:
                Debug.Log("North!");
                break;
            case DirectionValues.EAST:
                Debug.Log("East!");
                break;
            case DirectionValues.WEST:
                Debug.Log("West!");
                break;
            case DirectionValues.SOUTH:
                Debug.Log("South!");
                break;
            default:
                Debug.Log("No direction given!");
                break;
        }

        VideoController.ToNextVideo(direction);
    }
}
