using UnityEngine;
using System.Collections;

public class beatsManager : MonoBehaviour {

    public float bpm;
    private float secPerBeat;
    public GameObject p_beat;

	// Use this for initialization
	void Start () {
        // get time in seconds to spawn
        secPerBeat = 60f / bpm;
        InvokeRepeating("SpawnBeat", secPerBeat, secPerBeat);
	}
	
	// Update is called once per frame
	void Update () {
        secPerBeat = 60f / bpm;
    }

    void SpawnBeat()
    {
        p_beat.GetComponent<Transform>().localScale = gameObject.transform.localScale;
        var newBeat = GameObject.Instantiate(p_beat);
        Destroy(newBeat, 1f);
    }
}
