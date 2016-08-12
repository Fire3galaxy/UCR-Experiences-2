using UnityEngine;
using System.Collections;
using System;

public class WallVisibilityController : MonoBehaviour, IGvrGazeResponder {
    public GameObject Walls;

    public void OnGazeEnter()
    {
        
    }

    public void OnGazeExit()
    {
        
    }

    public void OnGazeTrigger()
    {
        Walls.SetActive(!Walls.activeSelf);
    }

    public void SetVisibility(bool active)
    {
        Walls.SetActive(active);
    }
}
