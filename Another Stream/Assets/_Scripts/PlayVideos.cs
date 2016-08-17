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
        initializeNodes();
        currNode = allNodes[0]; // Start index is 0

        Debug.Log("In start of PlayVideos: " + currNode.Link);
        mediaPlayer.OnReady += OnReady;
        mediaPlayer.OnEnd += OnEnd;
        mediaPlayer.OnVideoError += OnVideoError;
        mediaPlayer.Load(currNode.Link);
        
        eye.SetVisibility(false);   // Remove walls

        //StartCoroutine(DebugSeekPercent());
    }

    void OnReady()
    {
        sphere.GetComponent<Transform>().Rotate(new Vector3(0f, currNode.RotationY, 0f));
        duration = mediaPlayer.GetDuration();
        mediaPlayer.Play();
        
        Debug.Log("Duration: " + duration / 60000.0 + ", " + mediaPlayer.GetCurrentSeekPercent());
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

    private void initializeNodes()
    {
        // Links + Rotation value (0 to 360, counterclockwise)
        allNodes = new Node[] {
            new Node("https://www.dropbox.com/s/0bwkcq5a689f4kg/Flip_A.mp4?raw=1", 54.0f),
            new Node("https://www.dropbox.com/s/f2rwlvwcmglasa3/Flip_B.mp4?raw=1", 138.0f),
            new Node("https://www.dropbox.com/s/nt68w0zrisw3gbf/Flip_C.mp4?raw=1", 282.0f),
            new Node("https://www.dropbox.com/s/vjz0125bnloaozh/Flip_D.mp4?raw=1", 280.0f),
            new Node("https://www.dropbox.com/s/fahbyh28dnc4l0b/Flip_E.mp4?raw=1", 23.0f),
            new Node("https://www.dropbox.com/s/zxkkeewrn13cpu8/Flip_F.mp4?raw=1", 263.0f),
            new Node("https://www.dropbox.com/s/s6qy5bzmvi5h25w/Flip_G.mp4?raw=1", 171.0f)
        };

        // initialize directions
        allNodes[0].North = allNodes[1];
        allNodes[1].South = allNodes[0];
        allNodes[1].North = allNodes[2];
        allNodes[2].South = allNodes[1];
        allNodes[2].West = allNodes[3];
        allNodes[2].North = allNodes[5];
        allNodes[3].East = allNodes[2];
        allNodes[3].West = allNodes[4];
        allNodes[4].East = allNodes[3];
        allNodes[5].South = allNodes[2];
        allNodes[5].North = allNodes[6];
        allNodes[6].South = allNodes[5];
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

    IEnumerator DebugSeekPercent()
    {
        for (int i = 0; i < 15; i++)
        {
            yield return new WaitForSeconds(5);

            double percentageLoaded = mediaPlayer.GetCurrentSeekPercent() / 100.0;
            double percentagePlayed = (double)mediaPlayer.GetSeekPosition() / duration;
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
}
