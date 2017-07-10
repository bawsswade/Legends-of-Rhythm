using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class projectileSeek : MonoBehaviour {
    GameObject objToSeek;
    GameObject boss;     // for deflection
    public int maxLifetime;
    public float maxVel;
    public float maxSpeed;
    Vector3 desVel;
    float curDuration = 0;      // to not destroy projectile initially
    public Material deflectedMat;

    bool hasLaunched = false;
    int num = 0;
    float startY;
    public bool isDeflected;

    Rigidbody rb;
    BeatManagerMediator beatMan;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        if (!isDeflected)
        {
            objToSeek = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            objToSeek = GameObject.FindGameObjectWithTag("boss");
        }
        if(objToSeek == null)
        {
            Debug.Log("player not found");
        }
        startY = transform.position.y;
        beatMan = FindObjectOfType<BeatManagerMediator>();
        //rb.AddForce(transform.rotation.eulerAngles.normalized * 2);
        //Debug.Log(transform.rotation.eulerAngles.normalized);
	}
	
	// Update is called once per frame
	void Update () {
        if (curDuration < 1)
        {
            curDuration += Time.deltaTime;
        }
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

        transform.position = new Vector3(transform.position.x, startY, transform.position.z);
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

    void ChangeObjToSeek()
    {
        objToSeek = GameObject.FindGameObjectWithTag("boss");   // cahnge obj to seek
        gameObject.transform.LookAt(objToSeek.transform);   // change dir
        rb.AddForce(transform.forward * 800);       // add force
        hasLaunched = false;
        num = 0;
        gameObject.GetComponent<Renderer>().material = deflectedMat; // change mat
        curDuration = 0;
        isDeflected = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isDeflected)
        {
            // player loses health
            if (other.tag == "Player")
            {
                objToSeek.GetComponent<PlayerInputMediator>().MissedNote(objToSeek.transform);
                //Debug.Log(objToSeek.name);
                Destroy(gameObject);
            }
            // player hit note
            else if (other.tag == "hit" /*&& !objToSeek.GetComponent<PlayerInputView>().noteHit*/)
            {
                /*if (beatMan.CheckIsOnBeat())
                {
                    ChangeObjToSeek();
                }
                else
                {
                    Destroy(gameObject);
                }*/
                //objToSeek.GetComponent<PlayerInputMediator>().MissedNote(objToSeek.transform);
                Destroy(gameObject);
            }
        }
        else if(other.tag == "boss" && isDeflected)
        {
            //Destroy(gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "boss" && isDeflected)
        {
            //Destroy(gameObject);
        }
    }
}
