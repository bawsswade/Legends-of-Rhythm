using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Beatz;

public class BeatIndicator : MonoBehaviour {
    public Image circleImage;
    public int numBeats;
    public bool oneCycle;

    private float secPerBeat;
    private int count = 0;
	void Start () {
        secPerBeat = 60f / SongManager.bpm;

        InvokeRepeating("ChangeRotDir", 0, secPerBeat * numBeats);
	}
	
	void Update () {
        if (circleImage.fillClockwise)
        {
            circleImage.fillAmount += Time.deltaTime / (secPerBeat * numBeats);
        }
        else
        {
            circleImage.fillAmount -= Time.deltaTime / (secPerBeat * numBeats);
        }
    }

    private void ChangeRotDir()
    {
        if(oneCycle && count > 0)
        {
            Destroy(gameObject);
        }
        count++;

        circleImage.fillClockwise = !circleImage.fillClockwise;
    }
}
