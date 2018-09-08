using UnityEngine;
using System.Collections;
using System;
using Ins;
using Beatz;

// handles player input and sends to motor
public class player_input : MonoBehaviour {
    #region Variables
    public Animator anim;

    //public GameObject DashParts;
    //public Material onBeatMaterial;
    //private Material startMaterial;
    
    public bool isDashing = false;
    private bool isJumping = false;

    private player_motor motor;
    private player_abilities abilites;
    private bool shouldMove = true;

    private SongManager sm;
    private AudioSource source;

    private bool inNoteSync = false;

    // Action calls
    public Action<INPUTTYPE> OnDash;
    #endregion

    private void Start()
    {
        Ins.InuptManager.currentControls = CONTROLTYPE.PS4;
        
        motor = GetComponent<player_motor>();
        sm = GameObject.FindObjectOfType<Beatz.SongManager>();
        abilites = GetComponent<player_abilities>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //audio
        source = GetComponent<AudioSource>();

        // Actions
        abilites.OnEnterNoteSync += FreezeMovement;
        abilites.OnExitNoteSync += AllowMovement;
        OnDash += Dash;
    }

    private void Update()
    {
        // check for dash hits (constant beat)
        if(!inNoteSync)
        {
            DashInputs();
        }
        // check for specific note hits
        else
        {
            NoteHitInputs();
        }
    }

    private void DashInputs()
    {
        if (Ins.InuptManager.GetControls(INPUTTYPE.Atk1) && !isDashing && shouldMove && sm.GetIsOnBeat())
        {
            OnDash(INPUTTYPE.Atk1);
        }
        else if (Ins.InuptManager.GetControls(INPUTTYPE.Atk2) && !isDashing && shouldMove && sm.GetIsOnBeat())
        {
            OnDash(INPUTTYPE.Atk2);
        }

        if (Ins.InuptManager.GetControls(INPUTTYPE.LockOn))
        {
            //isLockedOn = !isLockedOn;
        }

        SetAnimation();
    }

    private void NoteHitInputs()
    {
        if (Ins.InuptManager.GetControls(INPUTTYPE.Atk1) && sm.GetHasHitNote(NoteType.TLeft))
        {
            source.Play();
            //Debug.Log("Hit left");
        }
        else if (Ins.InuptManager.GetControls(INPUTTYPE.Atk2) && sm.GetHasHitNote(NoteType.TRgiht))
        {
            source.Play();
            //Debug.Log("Hit right");
        }
    }

    private void Dash(INPUTTYPE type)
    {
        isDashing = true;
        anim.SetTrigger("isDashing");
        // get direction to dash
        //Vector3 x = transform.right * moveX * Time.deltaTime;
        //Vector3 z = transform.forward * moveY * Time.deltaTime;
        //Vector3 dir = (x + z).normalized;

        //transform.position += (x + z) * 50;
        StartCoroutine(WaitForEndDash());
    }

    IEnumerator WaitForEndDash()
    {
        yield return new WaitForSeconds(motor.dashTimer - sm.hitPadding);
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

    private void FreezeMovement()
    {
        shouldMove = false;
        inNoteSync = true;
    }

    private void AllowMovement()
    {
        shouldMove = true;
        inNoteSync = false;
    }
}
