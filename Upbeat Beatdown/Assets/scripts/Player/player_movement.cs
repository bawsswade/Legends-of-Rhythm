using UnityEngine;
using System.Collections;

public class player_movement : MonoBehaviour {
    private Rigidbody r;
    public float speed = 500f;
    // Use this for initialization
	void Start () {
        r = gameObject.GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    void Update()
    {
        // movement:  wasd
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        // use transform for changin local 
        Vector3 moveHor = transform.right * h;
        Vector3 moveVer = transform.forward * v;

        Vector3 force = (moveHor + moveVer).normalized * speed * Time.deltaTime;

        // jump: space
    }
}
