using UnityEngine;
using System.Collections;
using System;

public class PlayVideos : MonoBehaviour {

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
        
        eye.SetVisibility(false, null);   // Remove walls

        //StartCoroutine(DebugSeekPercent());
    }

    void OnReady()
    {
        Quaternion targetY = Quaternion.Euler(0f, currNode.RotationY, 0f);
        sphere.GetComponent<Transform>().transform.rotation = targetY;
        duration = mediaPlayer.GetDuration();
        mediaPlayer.Play();
        
        Debug.Log("Duration: " + duration / 60000.0 + ", " + mediaPlayer.GetCurrentSeekPercent());
    }

    void OnEnd ()
    {
        Debug.Log("In OnEnd");
        eye.SetVisibility(true, currNode);
    }

    void OnVideoError(MediaPlayerCtrl.MEDIAPLAYER_ERROR iCode, MediaPlayerCtrl.MEDIAPLAYER_ERROR iCodeExtra)
    {
        Debug.Log("In OnVideoError: " + mediaPlayer.GetDuration() / 1000 + ", " + mediaPlayer.GetSeekPosition() / 1000 + ", " + mediaPlayer.GetCurrentSeekPercent());
        eye.SetVisibility(true, currNode);
    }

    private void initializeNodes()
    {
        // Links + Rotation value (0 to 360, counterclockwise)
        allNodes = new Node[] {
            new Node("https://www.dropbox.com/s/d79qxmwhsn9cdeb/Flip_A.mp4?raw=1", 54.0f),
            new Node("https://www.dropbox.com/s/6y7f2ygf0nhy4uy/Flip_B.mp4?raw=1", 168.0f),
            new Node("https://www.dropbox.com/s/d8wg4l29g8nuw0m/Flip_C.mp4?raw=1", 182.0f),
            new Node("https://www.dropbox.com/s/t08fo4q86jxf9lr/Flip_D.mp4?raw=1", 200.0f),
            new Node("https://www.dropbox.com/s/npkc66647ku1efr/Flip_E.mp4?raw=1", 303.0f),
            new Node("https://www.dropbox.com/s/t4jf3gwuxsmj4uq/Flip_F.mp4?raw=1", 163.0f),
            new Node("https://www.dropbox.com/s/dul42z0hlkmi3z5/Flip_G.mp4?raw=1", 221.0f)
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
        eye.SetVisibility(false, null);
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
