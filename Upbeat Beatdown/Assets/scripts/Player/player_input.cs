using UnityEngine;
using System.Collections;
using System;
using Ins;
using Beatz;

// handles player input and sends to motor
public class player_input : MonoBehaviour {
    #region Variables
    public Animator anim;

    public GameObject DashParts;
    
    private bool isDashing = false;
    private bool isJumping = false;

    private player_motor motor;

    private SongManager sm;

    // Action calls
    public Action<INPUTTYPE, bool> OnDash;
    #endregion

    private void Start()
    {
        Ins.InuptManager.currentControls = CONTROLTYPE.PS4;
        
        motor = GetComponent<player_motor>();
        sm = GameObject.FindObjectOfType<Beatz.SongManager>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Actions
        OnDash += Dash;
    }

    private void Update()
    {
        if (Ins.InuptManager.GetControls(INPUTTYPE.Atk1) && !isDashing)
        {
            OnDash(INPUTTYPE.Atk1, sm.GetIsOnBeat());
            //motor.Dash(INPUTTYPE.Atk1, sm.GetIsOnBeat());
            //Dash(INPUTTYPE.Atk1);
        }
        else if (Ins.InuptManager.GetControls(INPUTTYPE.Atk2) && !isDashing)
        {
            OnDash(INPUTTYPE.Atk2, sm.GetIsOnBeat());
            //motor.Dash(INPUTTYPE.Atk2, sm.GetIsOnBeat());
            //Dash(INPUTTYPE.Atk2);
        }
        
        if (Ins.InuptManager.GetControls(INPUTTYPE.LockOn))
        {
            //isLockedOn = !isLockedOn;
        }

        SetAnimation();
    }

    private void Dash(INPUTTYPE type, bool isOnBeat)
    {
        isDashing = true;
        anim.SetTrigger("isDashing");

        //set parts
        DashParts.SetActive(true);
        var ps = DashParts.GetComponent<ParticleSystem>().main;
        Color temp = type == INPUTTYPE.Atk1 ? Color.red : Color.blue;
        temp.a = .15f;
        ps.startColor = temp;

        // get direction to dash
        //Vector3 x = transform.right * moveX * Time.deltaTime;
        //Vector3 z = transform.forward * moveY * Time.deltaTime;
        //Vector3 dir = (x + z).normalized;

        //transform.position += (x + z) * 50;
        StartCoroutine(WaitForEndDash());
    }

    IEnumerator WaitForEndDash()
    {
        yield return new WaitForSeconds(motor.dashTimer);
        DashParts.SetActive(false);
        isDashing = false;
    }

    private void SetAnimation()
    {
        if (motor.isMoving)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }

    private void Jump()
    {
        if (Ins.InuptManager.GetControls(INPUTTYPE.Jump) && !isJumping)
        {
            //motor.setJump(Vector3.up * 800);
            isJumping = true;
            //speed = 110;
        }
        else
        {
            //motor.setJump(Vector3.zero);
        }
    }
}
