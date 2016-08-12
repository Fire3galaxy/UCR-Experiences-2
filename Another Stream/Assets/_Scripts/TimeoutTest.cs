using UnityEngine;
using System.Collections;

public class TimeoutTest : MonoBehaviour {
    public GameObject door;

	void Start () {
        door.SetActive(false);
        //StartCoroutine(Example());
        StartCoroutine(Example2());
    }

    IEnumerator Example()
    {
        yield return new WaitForSeconds(2);
        door.SetActive(false);
        Debug.Log("After 2 seconds");
    }

    IEnumerator Example2()
    {
        yield return new WaitForSeconds(5);
        door.SetActive(true);
        Debug.Log("After 5 seconds");
    }
}
