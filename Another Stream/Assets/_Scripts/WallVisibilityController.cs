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

    public void SetVisibility(bool active, Node node)
    {
        // The whole wall gameobject
        Walls.SetActive(active);
        
        // Each individual direction
        if (active && node != null)
        {
            Debug.Log("In branch");
            foreach (Transform t in Walls.transform)
            {
                if (t.name.Equals("North"))
                    t.gameObject.SetActive(node.North != null);
                else if (t.name.Equals("East"))
                    t.gameObject.SetActive(node.East != null);
                else if (t.name.Equals("West"))
                    t.gameObject.SetActive(node.West != null);
                else
                    t.gameObject.SetActive(node.South != null);
            }
        }
    }
}
