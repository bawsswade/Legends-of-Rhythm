using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Beatz;

public class Launch : MonoBehaviour {

    public int force;
    public int duration;
    public int numBeatLifetime;

    private Rigidbody rb;
    private float timer = 0;

	// Use this for initialization
	void Start () {
        if (GetComponent<Rigidbody>() == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        else
        {
            rb = GetComponent<Rigidbody>();
        }

        Destroy(gameObject, (60f /SongManager.bpm) * numBeatLifetime);
	}
	
	// Update is called once per frame
	void Update () {
        if (timer < duration)
        {
            rb.AddForce(transform.up * force);
            timer += Time.deltaTime;
        }
	}
}
