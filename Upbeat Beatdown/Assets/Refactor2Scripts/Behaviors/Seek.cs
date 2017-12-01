using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : MonoBehaviour {

    public GameObject objToSeek;
    public float speed = 7;
    

    Rigidbody rb;
    BeatManagerMediator beatMan;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        rb.AddForce(GetSteer(objToSeek.transform.position));
    }

    Vector3 GetSteer(Vector3 target)
    {
        //Reynolds steering behaviour = desired - velocity
        //Should be global variables
        float maxForce = 4f;  //turning speed of the object

        //the direction that you need to go to reach the target
        Vector3 desired = target - transform.position;
        desired.Normalize();
        desired *= speed;

        //steering (reynolds steering)
        Vector3 steer = desired - rb.velocity;
        steer.y = 0f; //assuming you want 2D motion on the XZ plane

        //limit the steering
        if (steer.sqrMagnitude > maxForce * 40)
        {
            steer.Normalize();
            steer *= maxForce;
        }
        //transform.rotation = Quaternion.LookRotation(rb.velocity);

        return steer;
    }
}