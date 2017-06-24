using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class projectileSeek : MonoBehaviour {
    GameObject objToSeek;
    public int maxLifetime;
    public float maxVel;
    public float maxSpeed;
    Vector3 desVel;

    bool hasLaunched = false;
    int num = 0;

    Rigidbody rb;
    

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        objToSeek = GameObject.FindGameObjectWithTag("Player");
        if(objToSeek == null)
        {
            Debug.Log("player not found");
        }
        //rb.AddForce(transform.rotation.eulerAngles.normalized * 2);
        //Debug.Log(transform.rotation.eulerAngles.normalized);
	}
	
	// Update is called once per frame
	void Update () {
        if (!hasLaunched)
        {
            rb.AddForce(transform.forward * 40);
            num++;
            if (num > 20)
            {
                hasLaunched = true;
            }
        }
        else
        {
            rb.AddForce(Seek(objToSeek.transform.position));
        }
        //Destroy(gameObject, maxLifetime);


    }

    Vector3 Seek(Vector3 target)
    {
        //Reynolds steering behaviour = desired - velocity
        //Should be global variables
        float maxSpeed = 7f;    //fastest possible speed
        float maxForce = 4f;  //turning speed of the object

        //the direction that you need to go to reach the target
        Vector3 desired = target - transform.position;
        desired.Normalize();
        desired *= maxSpeed;

        //steering (reynolds steering)
        Vector3 steer = desired - rb.velocity;
        steer.y = 0f; //assuming you want 2D motion on the XZ plane

        //limit the steering
        if (steer.sqrMagnitude > maxForce * maxForce)
        {
            steer.Normalize();
            steer *= maxForce;
        }
        transform.rotation = Quaternion.LookRotation(rb.velocity);

        return steer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("lost health");
            Destroy(gameObject);
        }
        else if (other.tag == "hit")
        {
            //Debug.Log("note hit");
            Destroy(gameObject);
        }
    }
}
