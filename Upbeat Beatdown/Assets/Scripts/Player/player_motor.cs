using UnityEngine;
using System.Collections;
using Beatz;

public class player_motor : MonoBehaviour {
    #region Variables
    public int moveSpeed = 15;
    public float sensitivity;
    public GameObject model;
    public GameObject camPivot;
    public GameObject camera;
    public bool isLockedOn;
    public Transform lockedTarget;

    public bool isMoving = false;
    //public bool isDashing;
    private bool isJumping;
    private Rigidbody rb;
    private float lastRot;
    private float lookX, lookY, moveX, moveY;
    public float dashTimer;
    public float dashSpeed;
    private float curDashSpeed;
    private Beatz.SongManager sm;
    private player_input input;
    private player_abilities abilites;
    private bool inNoteSync = false;
    #endregion

    private void Start()
    {
        //Ins.InuptManager.currentControls = CONTROLTYPE.KEYBOARD;
        rb = GetComponent<Rigidbody>();
        input = GetComponent<player_input>();

        sm = GameObject.FindObjectOfType<Beatz.SongManager>();
        dashTimer = (60f / SongManager.bpm);
        //isDashing = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        abilites = GetComponent<player_abilities>();

        // add to input Actions
        input.OnDash += Dash;
        abilites.OnEnterNoteSync += FreezeMovement;
        abilites.OnExitNoteSync += AllowMovement;
    }

    private void Update()
    {
        moveX = Ins.InuptManager.GetAxis(INPUTTYPE.MoveX);
        moveY = Ins.InuptManager.GetAxis(INPUTTYPE.MoveY);
        lookX = Ins.InuptManager.GetAxis(INPUTTYPE.LookX);
        lookY = Ins.InuptManager.GetAxis(INPUTTYPE.LookY);

        // moving
        ApplyRotation();

        if (!input.isDashing && !inNoteSync)
        {
            ApplyForce();
        }

    }

    public void Dash(INPUTTYPE type)
    {
        // get direction to dash
        Vector3 x = camera.transform.right * moveX * Time.deltaTime;
        Vector3 z = camera.transform.forward * moveY * Time.deltaTime;
        Vector3 y = camera.transform.forward * Time.deltaTime;
        if(moveY < 0)
        {
            y = -y;
        }
        Vector3 dir = (x + z + y).normalized;

        curDashSpeed = dashSpeed;
        //curDashSpeed = isOnBeat ? dashSpeed : dashSpeed/2;
        StartCoroutine(StartDashTimer(dir, rb.velocity));
    }

    IEnumerator StartDashTimer(Vector3 _dir, Vector3 startVelocity)
    {
        float count = 0;
        Rigidbody r = GetComponent<Rigidbody>();
        //float speed = hasHit ? dashSpeed : dashSpeed / 2;
        while (count < dashTimer)
        {
            //transform.position += (_x + _z) * 30;
            r.velocity += (_dir * curDashSpeed);
            //r.AddForce((_x + _z) * 30);
            count += Time.deltaTime;

            // enable dashing when range of next hitPadding
            /*if (count > dashTimer - sm.hitPadding)
            {
                isDashing = false;
            }*/
        }
        yield return new WaitForSeconds(dashTimer - sm.hitPadding);
        //isDashing = false;
        rb.velocity = startVelocity;
    }

    private void ApplyForce()
    {
        Vector3 x = camPivot.transform.right * moveX * Time.deltaTime;
        Vector3 z = camPivot.transform.forward * moveY * Time.deltaTime;
        Vector3 dir = (x + z).normalized * moveSpeed;

        if ((Mathf.Abs(moveY) > .1f || Mathf.Abs(moveX) > .1f))
        {
            isMoving = true;
            transform.position += (x + z) * 15;

        }
        else
        {
            isMoving = false;
            rb.velocity = Vector3.zero;
        }
    }

    private void Jump()
    {
        if (!isJumping)
        {
            rb.velocity += (Vector3.up * 800);
            isJumping = true;
        }
        else
        {
            rb.velocity = (Vector3.up * 0);
        }
    }

    private void ApplyRotation()
    {
        if (isLockedOn)
        {
            transform.LookAt(lockedTarget);
            //model.transform.LookAt(lockedTarget.transform);
        }
        else
        {
            // rotate camera
            if (Mathf.Abs(lookY) > .1f || Mathf.Abs(lookX) > .1f)
            {
                // rotate camera
                camPivot.transform.Rotate(new Vector3(0, lookX, 0));

                //View.noteIndicators.transform.Rotate(new Vector3(0, lookX, 0));

                camPivot.transform.localEulerAngles = new Vector3(camPivot.transform.localEulerAngles.x - (lookY * sensitivity), 0, 0);
                transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + (lookX * sensitivity), 0);
            }

            //rotate player model
            if (Mathf.Abs(moveY) > .1f || Mathf.Abs(moveX) > .1f)
            {
                float angle = Mathf.Atan2(moveX, moveY) * Mathf.Rad2Deg;
                model.transform.localEulerAngles = new Vector3(0, angle, 0);
            }
        }

    }

    private void FreezeMovement()
    {
        inNoteSync = true;
        isLockedOn = true;
    }

    private void AllowMovement()
    {
        inNoteSync = false;
        isLockedOn = false;
    }
}
