using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using System;

public class PlayerMovementMediator : Mediator {

    [Inject] public PlayerMovementView View { get; set; }
<<<<<<< HEAD
    [Inject] public OnAttacking AttackingSignal { get; set; }

    public int speed = 15;
    //public int maxSpeed = 8;
    private player_motor motor;

    private Rigidbody rb;
    private float lastRot;
    private float lookX, lookY, moveX, moveY;
    public bool isJumping= false;
    private bool isAttacking = false;

    //public GameObject DashParts;
    private bool isDashing= false;
    private float dashTimer = .4f;
    private float dashSpeed = 1.5f;

    public override void OnRegister()
    {
        AttackingSignal.AddListener(SetAttacking);
=======

    public int speed = 100;
    public int maxSpeed = 8;

    private Rigidbody rb;
    private float lastRot;

	public override void OnRegister()
    {

>>>>>>> ae8663e27890825f99da0550b67eec12000e619c
	}

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
<<<<<<< HEAD
        motor = GetComponent<player_motor>();
=======
        
>>>>>>> ae8663e27890825f99da0550b67eec12000e619c
    }

    private void Update()
    {
<<<<<<< HEAD
        //View.camPivot.transform.LookAt(View.lockedTarget.transform);
        //transform.LookAt(View.lockedTarget.transform);

        moveX = Ins.InuptManager.GetAxis(INPUTTYPE.MoveX);
        moveY = Ins.InuptManager.GetAxis(INPUTTYPE.MoveY);
        lookX = Ins.InuptManager.GetAxis(INPUTTYPE.LookX);
        lookY = Ins.InuptManager.GetAxis(INPUTTYPE.LookY);

        // moving
        ApplyRotation();

        if (Ins.InuptManager.GetControls(INPUTTYPE.Dash))
        {
            Dash();
        }

        if (!isDashing)
        {
            ApplyForce();
        }

        View.camPivot.transform.position = transform.position;

        Jump();

        if (Ins.InuptManager.GetControls(INPUTTYPE.LockOn))
        {
            View.isLockedOn = !View.isLockedOn;
        }
    }

    private void Dash()
    {
        isDashing = true;
        View.DashParts.SetActive(true);

        Vector3 x = transform.right * moveX * Time.deltaTime;
        Vector3 z = transform.forward * moveY * Time.deltaTime;
        Vector3 dir = (x + z).normalized;
        //Debug.Log(dir);

        //transform.position += (x + z) * 50;
        StartCoroutine(StartDashTimer(dir));
    }

    IEnumerator StartDashTimer(Vector3 _dir)
    {
        float count = 0;
        Rigidbody r = GetComponent<Rigidbody>();
        while (count < dashTimer)
        {
            //transform.position += (_x + _z) * 30;
            r.velocity += (_dir * dashSpeed);
            //r.AddForce((_x + _z) * 30);
            count += Time.deltaTime;
        }
        yield return new WaitForSeconds(dashTimer);
        View.DashParts.SetActive(false);
        isDashing = false;
=======
        transform.LookAt(View.lockedTarget.transform);

        ApplyForce();
        ApplyRotation();
        
>>>>>>> ae8663e27890825f99da0550b67eec12000e619c
    }

    private void ApplyForce()
    {
<<<<<<< HEAD
        Vector3 x = transform.right * moveX * Time.deltaTime;
        Vector3 z = transform.forward * moveY * Time.deltaTime;
        Vector3 dir = (x + z).normalized * speed;

        if ((Mathf.Abs(moveY) > .1f || Mathf.Abs(moveX) > .1f) && !isAttacking)
        {
            //motor.setForce(dir);
            transform.position += (x+z) * 15;
            View.anim.SetBool("isRunning", true);
        }
        else
        {
            motor.setForce(Vector3.zero);
            View.anim.SetBool("isRunning", false);
        }
    }

    private void Jump()
    {
        if (Ins.InuptManager.GetControls(INPUTTYPE.Jump) && !isJumping)
        {
            motor.setJump(Vector3.up * 800);
            isJumping = true;
            speed = 110;
        }
        else
        {
            motor.setJump(Vector3.zero);
=======
        // movement: wasd
        float v = Input.GetAxis("PS4_L_Analog_Y");
        float h = Input.GetAxis("PS4_L_Analog_X");

        Vector3 direction = new Vector3(h, 0, v);

        if (v !=0|| h != 0)
        {
            rb.AddForce(direction * speed);
            //rb.AddForce(transform.forward * speed);
            //rb.AddForce(- transform.forward * (rb.velocity.magnitude - maxSpeed));
            //rb.AddForce(-direction * (rb.velocity.magnitude - maxSpeed));
        }
        else
        {
            //rb.AddForce(transform.forward * speed);
>>>>>>> ae8663e27890825f99da0550b67eec12000e619c
        }
    }

    private void ApplyRotation()
    {
<<<<<<< HEAD
        if (View.isLockedOn)
        {
            transform.LookAt(View.lockedTarget.transform);
            View.model.transform.LookAt(View.lockedTarget.transform);
        }
        else
        {
            

            // rotate camera
            if (Mathf.Abs(lookY) > .1f || Mathf.Abs(lookX) > .1f)
            {
                // rotate camera
                //View.camPivot.transform.Rotate(new Vector3(0, lookX, 0));

                //View.noteIndicators.transform.Rotate(new Vector3(0, lookX, 0));

                View.camPivot.transform.localEulerAngles = new Vector3(View.camPivot.transform.localEulerAngles.x - lookY, 0, 0);
                transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + lookX, 0);
            }

            //rotate player model
            if (Mathf.Abs(moveY) > .1f || Mathf.Abs(moveX) > .1f)
            {
                float angle = Mathf.Atan2(moveX, moveY) * Mathf.Rad2Deg;
                View.model.transform.localEulerAngles = new Vector3(0, angle, 0);
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "playerAttack")
        {
            isJumping = false;
            speed = 200;
        }
    }

    private void SetAttacking(bool b)
    {
        isAttacking = b;
    }

    private void AttackMove(float f )
    {

    }
=======
        // movement: wasd
        float v = Input.GetAxis("PS4_L_Analog_Y");
        float h = Input.GetAxis("PS4_L_Analog_X");

        float angle = Mathf.Atan2(h, v) * Mathf.Rad2Deg;

        if (Mathf.Abs(v) > .1f || Mathf.Abs(h) > .1f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            if(v == 1 || h == 1)
            {
                lastRot = angle;
            }
        }
        else
        {
            //transform.rotation = Quaternion.Euler(0, lastRot, 0);
        }
    }
>>>>>>> ae8663e27890825f99da0550b67eec12000e619c
}
