using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOE_Line : MonoBehaviour {

    public GameObject atkIndicator;
    //BeatManagerView beatMan;
    public GameObject rays;
    public GameObject wave1;
    public GameObject wave2;
    public int numIndicatorBeats;

    float secPerBeat;
    public float placementPadding = 8f;

	// Use this for initialization
	void Start () {
        //beatMan = GameObject.FindObjectOfType<BeatManagerView>();
        secPerBeat = 60 / Beatz.beatsManager.bpm;
        secPerBeat *= numIndicatorBeats;

        Invoke("Burst", secPerBeat);
	}
	
	// Update is called once per frame
	void Update () {
        if (atkIndicator.transform.localScale.x < 1)
        {
            atkIndicator.transform.localScale += new Vector3(Time.deltaTime/ secPerBeat, 1, 1);
            //transform.localScale = new Vector3(20,2, 1);
        }
	}

    void Burst()
    {
        atkIndicator.transform.localScale = new Vector3(1, 1, 1);
        rays.SetActive(true);
        wave1.SetActive(true);
        wave2.SetActive(true);
        atkIndicator.GetComponent<SpriteRenderer>().enabled = false;
        if (GetComponent<SphereCollider>() != null)
        {
            GetComponent<SphereCollider>().enabled = true;
        }
        else
        {
            GetComponent<BoxCollider>().enabled = true;
        }
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
            // if circle
            if (GetComponent<SphereCollider>())
            {
                GetComponent<SphereCollider>().enabled = false;
            }
            // line
            else
            {
                GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}
