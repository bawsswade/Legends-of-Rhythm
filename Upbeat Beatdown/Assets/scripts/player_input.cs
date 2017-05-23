using UnityEngine;
using System.Collections;

// handles player input and sends to motor
public class player_input : MonoBehaviour {
    public float movement_speed = 100f;
    public float rotation_speed = 10f;
    public float jumpStr = 100.0f;
    public GameObject attack;
    public GameObject right, left;

    private player_motor motor;
    private weaponManager weapon;
    private bool isGrounded;
    private bool canGlide;
    private bool glideTimerDone;

    private bool isLockedOn = false;

    // temp: change later
    public GameObject boss;
    public MusicAnalyzer ma;

    void Start () {
        motor = gameObject.GetComponent<player_motor>();
        weapon = gameObject.GetComponentInChildren<weaponManager>();
	}
	
	void Update () {
        // movement: wasd
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        // change locally
        
        Vector3 force, rotation, moveHor, moveVer;

        // rotate: mouse
        float rot = Input.GetAxisRaw("Mouse X");
        
        if (!isLockedOn)
        {
            moveHor = transform.right * h;
            moveVer = transform.forward * v;
            force = (moveHor + moveVer).normalized * movement_speed;
            rotation = new Vector3(0f, rot * rotation_speed, 0f);
        }
        else
        {
            // orbit
            moveHor = transform.right * h;
            moveVer = transform.forward * v;

            force = (moveHor + moveVer).normalized * movement_speed;
            rotation = new Vector3(0f, rot * rotation_speed, 0f);
        }

        motor.setRot(rotation);
        motor.setForce(force);
        
        // jump: space
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Vector3 jump = transform.up * jumpStr;
            motor.setJump(jump);
            glideTimerDone = false;
        }
        else
            motor.setJump(transform.up * 0);

        // glide
        if (weapon.equippedWeapon.name == "keytar" && !glideTimerDone)
        {
            canGlide = true;
        }
        else
        {
            canGlide = false;
        }

        if (Input.GetKey(KeyCode.Space) && !isGrounded && canGlide)
        {
            Gliding();
            // set glide to last 2 seconds
            Invoke("killGlide", 2.0f);
            Vector3 glide = transform.up * 12.0f;
            motor.setGlide(glide);
        }
        else
            motor.setGlide(transform.up * 0);

        // switch weapon
        if(Input.GetKeyDown(KeyCode.E))
        {
            weapon.SwitchWeapon(true);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            weapon.SwitchWeapon(false);
        }

        // lock on
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // find closest enemy?

            isLockedOn = !isLockedOn;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GameObject g = GameObject.Instantiate(attack, left.transform);
            g.transform.localPosition = Vector3.zero;
            g.transform.localRotation = Quaternion.identity;
            Destroy(g, 1f);
            if (ma.CheckBeat())
            {
                Debug.Log("hit");
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            GameObject g = GameObject.Instantiate(attack, right.transform);
            g.transform.localPosition = Vector3.zero;
            g.transform.localRotation = Quaternion.identity;
            Destroy(g, 1f);
            if (ma.CheckBeat())
            {
                Debug.Log("hit");
            }
        }

        if (isLockedOn)
        {
            transform.LookAt(boss.transform);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        isGrounded = true;
    }

    void OnTriggerExit(Collider col)
    {
        isGrounded = false;
    }

    void killGlide()
    {
        glideTimerDone = true;
    }

    void Gliding()
    {
        if (!glideTimerDone)
        {
            weapon.transform.position = gameObject.transform.position;
            weapon.transform.Translate(0, -.5f, 0);
            Debug.Log(gameObject.transform.position);
        }
    }
}
