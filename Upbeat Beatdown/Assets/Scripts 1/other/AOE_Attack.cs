using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOE_Attack : MonoBehaviour {

    public GameObject atkIndicator;
<<<<<<< HEAD
    Beatz.beatsManager beatMan;
=======
    BeatManagerView beatMan;
>>>>>>> ae8663e27890825f99da0550b67eec12000e619c
    public GameObject particles;
    public int numIndicatorBeats;

    float secPerBeat;
    public float placementPadding = 5f;

	// Use this for initialization
	void Start () {
<<<<<<< HEAD
        //beatMan = GameObject.FindObjectOfType<beatz.>();
        secPerBeat = 60 / 140.0f;
=======
        beatMan = GameObject.FindObjectOfType<BeatManagerView>();
        secPerBeat = 60 / beatMan.bpm;
>>>>>>> ae8663e27890825f99da0550b67eec12000e619c
        secPerBeat *= numIndicatorBeats;

        Invoke("Burst", secPerBeat);
	}
	
	// Update is called once per frame
	void Update () {
        if (atkIndicator.transform.localScale.x < 1)
        {
            atkIndicator.transform.localScale += new Vector3(Time.deltaTime/ secPerBeat, 1, Time.deltaTime / secPerBeat);
        }
	}

    void Burst()
    {
        atkIndicator.transform.localScale = new Vector3(1, 1, 1);
        particles.SetActive(true);
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
