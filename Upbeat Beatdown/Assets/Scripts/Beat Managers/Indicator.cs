using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour {
    public bool limitBeats;
    public int numBeats;
    public int bpm;
    public bool shouldTrackObject;
    public GameObject objToTrack;
    public List<GameObject> ActiveIndicators;

    //beat counter vars
    private int index = -1;
    private int curNumBeats = 0;

    // indicator vars
    private Camera camera;
    private RectTransform rectTrans;
	
	void Start () {
        // set tracking
        if (shouldTrackObject)
        {
            camera = Camera.main;
            //Debug.Log(camera.name);
            rectTrans = (RectTransform)GameObject.Find("HUD").transform;
        }

        // start on spawn
        InvokeRepeating("Increment", 0, 60f/bpm);
	}

    private void Update()
    {
        // uncomment to track target
        /*
        //adust position
        Vector3 ViewportPosition = camera.WorldToViewportPoint(new Vector3(objToTrack.transform.position.x, objToTrack.transform.position.y, objToTrack.transform.position.z));
        if (ViewportPosition.x > -.5f && ViewportPosition.x < 1.5 &&
            ViewportPosition.y > -.5f && ViewportPosition.y < 1.5)
        {
            if (ViewportPosition.z > 0)
            {
                Vector2 WorldObject_ScreenPosition = new Vector2(
                ((ViewportPosition.x * rectTrans.rect.width) - (rectTrans.rect.width * 0.5f)),
                ((ViewportPosition.y * rectTrans.rect.height) - (rectTrans.rect.height * 0.5f)));
                transform.localPosition = WorldObject_ScreenPosition;
            }
        }
        // adjust size
        float distance = (objToTrack.transform.position - camera.transform.position).magnitude;
        //Debug.Log(distance);
        if (distance > 0)
        {
            distance *= .1f;
            transform.localScale = new Vector3(1 / distance, 1 / distance, 1 / distance);
        }*/
    }

    void Increment()
    {
        if (index != -1)
        {
            ActiveIndicators[index].SetActive(false);
        }

        index++;
        if (index > ActiveIndicators.Count -1)
        {
            index = 0;
        }
        ActiveIndicators[index].SetActive(true);

        if (limitBeats)
        {
            curNumBeats++;
            if (curNumBeats > numBeats)
            {
                Destroy(gameObject);
            }
        }
    }
}
