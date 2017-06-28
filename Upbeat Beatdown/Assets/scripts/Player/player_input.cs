using UnityEngine;
using System.Collections;
using Ins;

// handles player input and sends to motor
public class player_input : MonoBehaviour {
    public float movement_speed = 100f;
    public float dashForce = 500f;
    public float rotation_speed = 10f;
    public float jumpStr = 100.0f;
    public float dashPadding = .7f;
    public float dashDuration = .01f;
    public bool noteHit;

    public float attackDuration;
    bool isDashing = false;
    float dashTimer = 0;
    Vector3 force = Vector3.zero;

    float l_attackTimer = 0;
    bool l_isAttacking;
    float r_attackTimer = 0;
    bool r_isAttacking;

    public GameObject l_attack, r_attack;   // enable mesh renerer for visual animation
    public GameObject hit_part;
    public Transform l_hitPos, r_hitPos;
    public BoxCollider l_boxCol, r_boxCol;  // enable collider
    public GameObject right, left;

    private player_motor motor;
    private weaponManager weapon;
    private bool isGrounded;
    private bool canGlide;
    private bool glideTimerDone;

    private bool isLockedOn = false;

    // temp: change later
    public GameObject boss;
    public beatsManager beatMan;

    void Start () {
        motor = gameObject.GetComponent<player_motor>();
        weapon = gameObject.GetComponentInChildren<weaponManager>();
	}
	
	void Update () {
        // movement: wasd
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        // change locally
        
        Vector3 rotation, moveHor, moveVer;

        // rotate: mouse
        float rot = Input.GetAxisRaw("Mouse X");
        
        if (!isLockedOn)
        {
            moveHor = transform.right * h;
            moveVer = transform.forward * v;
            //force = (/*moveHor +*/ moveVer).normalized * movement_speed;
            rotation = new Vector3(0f, rot * rotation_speed, 0f);
        }
        else
        {
            // orbit
            //moveHor = transform.right * h;
            moveVer = transform.forward * v;

            //force = (/*moveHor +*/ moveVer).normalized * movement_speed;
            rotation = new Vector3(0f, rot * rotation_speed, 0f);
        }

        // dashing
        if (Ins.InuptManager.GetAxis(INPUTTYPE.AtkRight) != 1 && Ins.InuptManager.GetAxis(INPUTTYPE.AtkLeft) != 1 &&
            Mathf.Abs(Ins.InuptManager.GetAxis(INPUTTYPE.AtkRight) - Ins.InuptManager.GetAxis(INPUTTYPE.AtkLeft)) < dashPadding
            && !isDashing)
        {
            // CHANGE TO DASH TOWARDS NOTES
            moveHor = transform.right * ((Ins.InuptManager.GetAxis(INPUTTYPE.MoveX) + Ins.InuptManager.GetAxis(INPUTTYPE.LookX))/2);
            moveVer = transform.forward * ((Ins.InuptManager.GetAxis(INPUTTYPE.MoveY) + Ins.InuptManager.GetAxis(INPUTTYPE.LookY)) / 2);
            force = (moveHor +moveVer).normalized * dashForce;
            isDashing = true;
            dashTimer = 0;
            noteHit = beatMan.IsOnBeat();
            
        }
        //Debug.Log(Ins.InuptManager.GetAxis(INPUTTYPE.AtkLeft) + " / " + Ins.InuptManager.GetAxis(INPUTTYPE.AtkRight));
        motor.setRot(rotation);
        // dashing
        if (isDashing)
        {
            dashTimer += Time.deltaTime;
        }
        if ((dashTimer < dashDuration) && isDashing)
        {
            //Debug.Log(dashTimer + "/" + dashDuration);
            motor.setForce(force);

            
        }
        else
        {
            //Debug.Log("stop");
            force = Vector3.zero;
            motor.setForce(force);
            
        }
        if((dashTimer < dashDuration + .2))
        {
            l_attack.SetActive(true);
            l_boxCol.enabled = true;
            r_attack.SetActive(true);
            r_boxCol.enabled = true;
        }
        else
        {
            l_attack.SetActive(false);
            l_boxCol.enabled = false;
            r_attack.SetActive(false);
            r_boxCol.enabled = false;
        }
        
        if (Ins.InuptManager.GetAxis(INPUTTYPE.AtkRight) == 1 && Ins.InuptManager.GetAxis(INPUTTYPE.AtkLeft) == 1 && dashTimer > dashDuration+.3)
        {
            isDashing = false;
        }
        
        /* jump: space
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
        }*/

        // lock on
        if (Input.GetKeyDown(KeyCode.Return) || Ins.InuptManager.GetControls(INPUTTYPE.LockOn))
        {
            // find closest enemy?
            //Debug.Log("locked on");
            isLockedOn = !isLockedOn;
        }
       

        // left attack
        if (Ins.InuptManager.GetAxis(INPUTTYPE.AtkLeft) != 1 && !l_isAttacking) // set timer
        {
            l_attackTimer = 0;
            l_isAttacking = true;
            noteHit = beatMan.IsOnBeat();
        }
        else if (!isDashing )
        {
            l_attack.SetActive(false);
            l_boxCol.enabled = false;
        }
        if (l_isAttacking)
        {
            l_attackTimer += Time.deltaTime;
            // activate hitbox
            if(l_attackTimer < attackDuration)
            {
                l_attack.SetActive(true);
                l_boxCol.enabled = true;
            }
        }
        // reset when button released
        if(Ins.InuptManager.GetAxis(INPUTTYPE.AtkLeft) == 1 && l_attackTimer > attackDuration)
        {
            l_isAttacking = false;
        }

        // right attack
        if (Ins.InuptManager.GetAxis(INPUTTYPE.AtkRight) != 1 && !r_isAttacking) // set timer
        {
            r_attackTimer = 0;
            r_isAttacking = true;
            noteHit = beatMan.IsOnBeat();
        }
        else if(!isDashing)
        {
            r_attack.SetActive(false);
            r_boxCol.enabled = false;
        }
        if (r_isAttacking)
        {
            r_attackTimer += Time.deltaTime;
            // activate hitbox
            if (r_attackTimer < attackDuration)
            {
                r_attack.SetActive(true);
                r_boxCol.enabled = true;
            }
        }
        // reset when button released
        if (Ins.InuptManager.GetAxis(INPUTTYPE.AtkRight) == 1 && r_attackTimer > attackDuration)
        {
            r_isAttacking = false;
            //noteHit = false;
        }

        if (isLockedOn)
        {
            transform.LookAt(boss.transform);
        }

        if (!isDashing && !r_isAttacking && !l_isAttacking)
        {
            noteHit = false;
        }
    }

    // for jumps and glides later on
    /*void OnTriggerEnter(Collider col)
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
    }*/

    public void Left_NoteHit()
    {
        GameObject g = Instantiate(hit_part, l_hitPos);
        g.transform.localPosition = Vector3.zero;
        Destroy(g,1);
    }

    public void Right_NoteHit()
    {
        GameObject g = Instantiate(hit_part, r_hitPos);
        g.transform.localPosition = Vector3.zero;
        Destroy(g, 1);
    }
}
