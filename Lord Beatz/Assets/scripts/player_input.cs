using UnityEngine;
using System.Collections;

public class player_input : MonoBehaviour {
    public float movement_speed = 100f;
    public float rotation_speed = 10f;
    public float jumpStr = 50f;
    private player_motor motor;
    
    void Start () {
        motor = gameObject.GetComponent<player_motor>();
	}
	
	void Update () {
        // movement: wasd
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        // change locally
        Vector3 moveHor = transform.right * h;
        Vector3 moveVer = transform.forward * v;

        Vector3 force = (moveHor + moveVer).normalized * movement_speed;
        
        motor.setForce(force);

        // rotate: mouse
        float rot = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0f, rot * rotation_speed, 0f);

        motor.setRot(rotation);

        
        // jump: space
        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 jump = transform.up * jumpStr;
            motor.setJump(jump);
        }
        else
            motor.setJump(transform.up * 0);
    }
}
