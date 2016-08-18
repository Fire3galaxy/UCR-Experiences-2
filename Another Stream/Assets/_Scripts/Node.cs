using UnityEngine;
using System.Collections;
using System;

public class Node {
    public String Link;
    public Node North, South, East, West;
    public float RotationY;

    public Node(String link, float rotationY)
    {
        Link = link;
        North = null;
        South = null;
        East = null;
        West = null;
        RotationY = rotationY;
    }
}
