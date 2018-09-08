using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Beatz;

public class RotatingIdicator : MonoBehaviour {
    public GameObject indicator;

    private float rot = 0;
    private float secPerBeat = 60f / SongManager.bpm;

    void Update ()
    {
        rot -= (Time.deltaTime *  360) / (secPerBeat * 4);
        indicator.transform.localEulerAngles =  new Vector3(0, 0, rot);
	}
}
