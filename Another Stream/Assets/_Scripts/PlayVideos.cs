using UnityEngine;
using System.Collections;
using System;

public class PlayVideos : MonoBehaviour {
    class Node
    {
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

    public MediaPlayerCtrl mediaPlayer;
    public WallVisibilityController eye;
    public GameObject sphere;
    
    private Node[] allNodes;
    private Node currNode;
    private int duration;

    void Start ()
    {
        // Links + Rotation value (0 to 360, counterclockwise)
        // FIXME: Change first video so pathway of video is north
        allNodes = new Node[] {
            new Node("https://www.dropbox.com/s/ztqvxo4asboxpyu/scene%20A%20stitched.mp4?raw=1", 39.0f),
            new Node("http://kolor.com/360-videos-files/kolor-stearman-biplane-icare-full-hd.mp4", 0.0f),
            new Node("http://kolor.com/360-videos-files/assets/Apartment_tour.mp4", 0.0f)
        };

        // Apartments <-- Rio --> Biplane (initialize directions)
        allNodes[0].East = allNodes[1];
        allNodes[0].West = allNodes[2];
        allNodes[1].West = allNodes[0];
        allNodes[2].East = allNodes[0];

        // Start index is 0
        currNode = allNodes[0];

        Debug.Log("In start of PlayVideos: " + currNode.Link);
        mediaPlayer.OnReady += OnReady;
        mediaPlayer.OnEnd += OnEnd;
        mediaPlayer.OnVideoError += OnVideoError;
        mediaPlayer.Load(currNode.Link);
        
        eye.SetVisibility(false);   // Remove walls

        //StartCoroutine(DebugSeekPercent());
    }

    IEnumerator DebugSeekPercent()
    {
        for (int i = 0; i < 15; i++)
        {
            yield return new WaitForSeconds(5);

            double percentageLoaded = mediaPlayer.GetCurrentSeekPercent() / 100.0;
            double percentagePlayed = (double) mediaPlayer.GetSeekPosition() / duration;
            double difference = Math.Abs(percentageLoaded - percentagePlayed);
            //Debug.Log("Debug: Seek % = " + mediaPlayer.GetCurrentSeekPercent() + " at position " + mediaPlayer.GetSeekPosition());
            Debug.Log("Debug: Percentage Loaded: " + percentageLoaded + ", Percentage Played: " + percentagePlayed);

            //if (percentagePlayed > .05 && difference < .05 && 
            //        mediaPlayer.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.PAUSED)
            //{
            //    mediaPlayer.Pause();
            //    Debug.Log("Debug: Paused!");
            //}

            //if (difference > .1 &&
            //        mediaPlayer.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PAUSED)
            //    mediaPlayer.Play();

            //Debug.Log("Debug: difference = " + difference);
        }
    }

    void OnReady()
    {
        mediaPlayer.Play();
        sphere.GetComponent<Transform>().Rotate(new Vector3(0f, currNode.RotationY, 0f));
        duration = mediaPlayer.GetDuration();
        Debug.Log("Duration: " + mediaPlayer.GetDuration() / 60000.0 + ", " + mediaPlayer.GetCurrentSeekPercent());
    }

    void OnEnd ()
    {
        Debug.Log("In OnEnd");
        eye.SetVisibility(true);
    }

    void OnVideoError(MediaPlayerCtrl.MEDIAPLAYER_ERROR iCode, MediaPlayerCtrl.MEDIAPLAYER_ERROR iCodeExtra)
    {
        Debug.Log("In OnVideoError: " + mediaPlayer.GetDuration() / 1000 + ", " + mediaPlayer.GetSeekPosition() / 1000 + ", " + mediaPlayer.GetCurrentSeekPercent());
        eye.SetVisibility(true);
    }

    public void ToNextVideo (int direction)
    {   
        switch (direction)
        {
            case DirectionValues.NORTH:
                if (currNode.North != null)
                {
                    currNode = currNode.North;
                    PlayVideo(currNode.Link);
                }
                break;
            case DirectionValues.EAST:
                if (currNode.East != null)
                {
                    currNode = currNode.East;
                    PlayVideo(currNode.Link);
                }
                break;
            case DirectionValues.WEST:
                if (currNode.West != null)
                {
                    currNode = currNode.West;
                    PlayVideo(currNode.Link);
                }
                break;
            case DirectionValues.SOUTH:
                if (currNode.South != null)
                {
                    currNode = currNode.South;
                    PlayVideo(currNode.Link);
                }
                break;
            default:
                Debug.Log("Bad direction specified.");
                break;
        }
    }

    private void PlayVideo(String Link)
    {
        eye.SetVisibility(false);
        mediaPlayer.UnLoad();
        mediaPlayer.Load(Link);
        mediaPlayer.Play();
    }
}
