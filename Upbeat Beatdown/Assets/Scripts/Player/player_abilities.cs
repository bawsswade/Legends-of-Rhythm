using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Beatz;
using UnityEngine.UI;

public class player_abilities : MonoBehaviour {

    private SphereCollider dashCol;
    private player_input input;
    private player_motor motor;
    public Slider meter;

	void Start ()
    {
        if (GetComponent<SphereCollider>() == null)
        {
            SphereCollider col =  gameObject.AddComponent<SphereCollider>();
            col.center = new Vector3(0,.1f,0);
            col.radius = 2.25f;
        }
        else
        {
            dashCol = GetComponent<SphereCollider>();
        }

        motor = GetComponent<player_motor>();

        dashCol.enabled = false;
        input = GetComponent<player_input>();

        input.OnDash += Dash;
	}
	
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Note")
        {
            other.GetComponent<P1Note>().HasHitNote();
            meter.value += 5;
        }
    }

    private void Dash(INPUTTYPE type, bool isOnBeat)
    {
        if (isOnBeat)
        {
            dashCol.enabled = true;
            StartCoroutine( "WaitForEndDash");
        }
    }

    IEnumerator WaitForEndDash()
    {
        yield return new WaitForSeconds(motor.dashTimer);
        dashCol.enabled = false;
    }
}
