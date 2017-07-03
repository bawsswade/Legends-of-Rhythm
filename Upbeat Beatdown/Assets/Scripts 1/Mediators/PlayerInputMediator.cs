﻿using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using System;

public class PlayerInputMediator : Mediator {
    public float movement_speed = 100f;
    public float dashForce = 500f;
    public float rotation_speed = 10f;
    public float jumpStr = 100.0f;
    public float dashPadding = .4f;     // how close both axis need to be to dash
    public float dashDuration = .13f;
    float minDashValue = .3f;           // min value for analog stick 

    public float attackDuration = .2f;
    float specialAtkCharge = 0;
    float chargeIncrement = 2;
    bool canSpecialAtk = false;
    bool hasCharged = false;
    float specialStartSize;
    
    float dashTimer = 0;
    Vector3 force = Vector3.zero;

    float l_attackTimer = 0;
    bool l_isAttacking;
    float r_attackTimer = 0;
    bool r_isAttacking;

    private player_motor motor;

    private bool isLockedOn = false;


    [Inject] public PlayerInputView View { get; set; }
    // player stuffs
    [Inject] public OnRightAttackSignal RightAtkSignal { get; set; }
    [Inject] public OnLeftAttackSignal LeftAtkSignal { get; set; }
    [Inject] public OnDashSignal DashSignal { get; set; }
    [Inject] public OnChargeSpecial ChargeSpecialSignal { get; set; }
    [Inject] public OnRightResetHit ResetRightSignal { get; set; }
    [Inject] public OnLeftResetHit ResetLeftSignal { get; set; }
    //spawn hit particles
    [Inject] public OnLeftHit LeftHitSignal { get; set; }
    [Inject] public OnRightHit RightHitSignal { get; set; }

    public override void OnRegister()
    {
        // for setting onBeat
        //BassBeatSignal.AddListener(BassBeat);
        //MelodyBeatSignal.AddListener(MelodyBeat);
        // create hit particle
        LeftHitSignal.AddListener(Left_NoteHit);
        RightHitSignal.AddListener(Right_NoteHit);
        ChargeSpecialSignal.AddListener(IncreaseCharge);
	}
    
    void Start()
    {
        motor = gameObject.GetComponent<player_motor>();

        specialStartSize = View.spAtkIndicator.transform.localScale.x;
        View.spAtkIndicator.transform.localScale = Vector3.zero;
    }

    void Update()
    {
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
            rotation = new Vector3(0f, rot * rotation_speed, 0f);
        }
        else
        {
            // orbit
            moveVer = transform.forward * v;
            
            rotation = new Vector3(0f, rot * rotation_speed, 0f);
        }

        // dashing
        if (Ins.InuptManager.GetControls(INPUTTYPE.Dash) && !View.isDashing)                         // how close left and right atk value have to be
        {
            // CHANGE TO DASH TOWARDS NOTES
            moveHor = transform.right * ((Ins.InuptManager.GetAxis(INPUTTYPE.MoveX) + Ins.InuptManager.GetAxis(INPUTTYPE.LookX)) / 2);
            moveVer = transform.forward * ((Ins.InuptManager.GetAxis(INPUTTYPE.MoveY) + Ins.InuptManager.GetAxis(INPUTTYPE.LookY)) / 2);
            force = (moveHor + moveVer).normalized * dashForce;
            View.isDashing = true;
            dashTimer = 0;
            View.noteHit = View.beatMan.GetComponent<BeatManagerMediator>().CheckIsOnBeat();  // only check on melody

            GameObject g = Instantiate(View.dashParticles, transform);
            //g.transform.rotation = Quaternion.Euler(-force);
            Destroy(g,.2f);
        }

        motor.setRot(rotation);

        // dashing
        if (View.isDashing)
        {
            dashTimer += Time.deltaTime;
        }
        if ((dashTimer < dashDuration) && View.isDashing)
        {
            motor.setForce(force);


        }
        else
        {
            force = Vector3.zero;
            motor.setForce(force);

        }
        if ((dashTimer < dashDuration + .2))
        {
            View.l_attack.SetActive(true);
            View.l_boxCol.enabled = true;
            View.r_attack.SetActive(true);
            View.r_boxCol.enabled = true;
        }
        else
        {
            View.l_attack.SetActive(false);
            View.l_boxCol.enabled = false;
            View.r_attack.SetActive(false);
            View.r_boxCol.enabled = false;
            hasCharged = false;
        }

        if (Ins.InuptManager.GetAxis(INPUTTYPE.AtkRight) == 1 && Ins.InuptManager.GetAxis(INPUTTYPE.AtkLeft) == 1 && dashTimer > dashDuration + .3)
        {
            View.isDashing = false;
        }

        // lock on
        if (Input.GetKeyDown(KeyCode.Return) || Ins.InuptManager.GetControls(INPUTTYPE.LockOn))
        {
            isLockedOn = !isLockedOn;
        }


        // left attack
        if (Ins.InuptManager.GetAxis(INPUTTYPE.AtkLeft) != 1 && !l_isAttacking) // set timer
        {
            l_attackTimer = 0;
            l_isAttacking = true;
            View.noteHit = View.beatMan.GetComponent<BeatManagerMediator>().CheckIsOnBeat();     // only checks on melody for now
            if (!View.noteHit)
            {
                //Debug.Log("l_missed");
                MissedNote(View.l_hitPos);
            }
        }
        else if (!View.isDashing)
        {
            View.l_attack.SetActive(false);
            View.l_boxCol.enabled = false;
        }
        if (l_isAttacking)
        {
            l_attackTimer += Time.deltaTime;
            // activate hitbox
            if (l_attackTimer < attackDuration)
            {
                View.l_attack.SetActive(true);
                View.l_boxCol.enabled = true;
            }
        }
        // reset when button released
        if (Ins.InuptManager.GetAxis(INPUTTYPE.AtkLeft) == 1 && l_attackTimer > attackDuration)
        {
            l_isAttacking = false;
            ResetLeftSignal.Dispatch();
        }

        // right attack
        if (Ins.InuptManager.GetAxis(INPUTTYPE.AtkRight) != 1 && !r_isAttacking) // set timer
        {
            r_attackTimer = 0;
            r_isAttacking = true;
            View.noteHit = View.beatMan.GetComponent<BeatManagerMediator>().CheckIsOnBeat();     // only checks  for melody
            if (!View.noteHit)
            {
                //Debug.Log("missed");
                MissedNote(View.r_hitPos);
            }
        }
        else if (!View.isDashing)
        {
            View.r_attack.SetActive(false);
            View.r_boxCol.enabled = false;
        }
        if (r_isAttacking)
        {
            r_attackTimer += Time.deltaTime;
            // activate hitbox
            if (r_attackTimer < attackDuration)
            {
                View.r_attack.SetActive(true);
                View.r_boxCol.enabled = true;
            }
        }
        // reset when button released
        if (Ins.InuptManager.GetAxis(INPUTTYPE.AtkRight) == 1 && r_attackTimer > attackDuration)
        {
            r_isAttacking = false;
            ResetRightSignal.Dispatch();        // this works...but left doesnt
        }

        if (isLockedOn)
        {
            transform.LookAt(View.boss.transform);
        }

        if (!View.isDashing && !r_isAttacking && !l_isAttacking)
        {
            View.noteHit = false;

        }

        if (canSpecialAtk && Ins.InuptManager.GetControls(INPUTTYPE.SpecialAtk))
        {
            View.specialAtk.SetActive(true);
            Invoke("ResetSpecial", 2);
        }
        else if(!canSpecialAtk)
        {
            View.specialAtk.SetActive(false);
        }
    }

    public void Left_NoteHit()
    {
        GameObject g = Instantiate(View.hit_part, View.l_hitPos.parent);
        g.transform.localPosition = new Vector3(0,-1.2f,0);
        g.transform.localRotation = Quaternion.Euler( 90,0,0);
        Destroy(g, 1);

        if (!View.isDashing)
        {
            GameObject d = Instantiate(View.deflectProjectile, View.l_hitPos.parent);
            d.transform.localPosition = Vector3.zero;
            d.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

    public void Right_NoteHit()
    {
        GameObject g = Instantiate(View.hit_part, View.r_hitPos.parent);
        g.transform.localPosition = new Vector3(0, -1.2f, 0);
        g.transform.localRotation = Quaternion.Euler(90,0,0);
        Destroy(g, 1);

        if (!View.isDashing)
        {
            GameObject d = Instantiate(View.deflectProjectile, View.r_hitPos.parent);
            d.transform.localPosition = Vector3.zero;
            d.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

    public void MissedNote(Transform parent)
    {
        GameObject g;
        if (parent.parent == null)
        {
            g = Instantiate(View.missParticles, parent);
        }
        else
        {
            g = Instantiate(View.missParticles, parent.parent);
        }
        g.transform.localPosition = new Vector3(0, -1.2f, 0);
        g.transform.localRotation = Quaternion.Euler(90, 0, 0);
        Destroy(g, 1);
    }

    private void IncreaseCharge()
    {
        if (specialAtkCharge < 100)
        {
            // increment
            specialAtkCharge += chargeIncrement;
            View.spAtkIndicator.transform.localScale = new Vector3(specialStartSize*(specialAtkCharge/100), 1, specialStartSize * (specialAtkCharge / 100));
            View.specialAtkText.text = specialAtkCharge.ToString();
            hasCharged = true;
        }
        if(specialAtkCharge >= 100)
        {
            canSpecialAtk = true;
        }
    }

    void ResetSpecial()
    {
        canSpecialAtk = false;
        specialAtkCharge = 0;
        View.specialAtkText.text = specialAtkCharge.ToString();
        View.spAtkIndicator.transform.localScale = Vector3.zero;
    }
}
