using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOE_Attack : MonoBehaviour {

    public GameObject atkIndicator;
    BeatManagerView beatMan;
    public GameObject particles;
    public int numIndicatorBeats;

    float secPerBeat;
    public float placementPadding = 5f;

	// Use this for initialization
	void Start () {
        beatMan = GameObject.FindObjectOfType<BeatManagerView>();
        secPerBeat = 60 / beatMan.bpm;
        secPerBeat *= numIndicatorBeats;

        Invoke("Burst", secPerBeat);
	}
	
	// Update is called once per frame
	void Update () {
        if (atkIndicator.transform.localScale.x < 1)
        {
            atkIndicator.transform.localScale += new Vector3(Time.deltaTime/ secPerBeat, Time.deltaTime / secPerBeat, Time.deltaTime / secPerBeat);
        }
	}

    void Burst()
    {
        atkIndicator.transform.localScale = new Vector3(1, 1, 1);
        particles.SetActive(true);
        atkIndicator.GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<SphereCollider>().enabled = true;
        Invoke("Deactivate", .7f);
    }

    void Deactivate()
    {
        atkIndicator.transform.parent.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "hit")
        {
            GetComponent<SphereCollider>().enabled = false;
        }
    }
}
