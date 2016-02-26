using UnityEngine;
using System.Collections;

public class projLaunch : MonoBehaviour {
    private Rigidbody r;
    public float speed = 15;
	
	void Start () {
        r = gameObject.GetComponent<Rigidbody>();
        r.AddForce(transform.forward * speed);
    }

	void Update () {
        
        //Destroy(gameObject, 8f);
	}
}
