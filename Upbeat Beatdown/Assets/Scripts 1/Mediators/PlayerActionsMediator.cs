using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using System;

public class PlayerActionsMediator : Mediator {

    [Inject] public PlayerActionsView View { get; set; }
    [Inject] public OnChangeNoteType NoteChangeSignal { get; set; }
    [Inject] public OnAttacking StopMovementSignal { get; set; }
    [Inject] public OnEnemyInRange EnemyInRangeSignal { get; set; }

    public Weapon curWeapon;        // slot
    public float attackTime = .5f;
    public float atkMoveTime = .12f;
    private float atkTimer = 0;
    private float distFromEnemy;
    private Vector3 enemyPos;

    public NOTETYPE curNoteType;
    private bool hasAttack = false;
    private IEnumerator rout;

	public override void OnRegister()
    {
        NoteChangeSignal.AddListener(ChangeNoteType);
        EnemyInRangeSignal.AddListener(SetDistanceFromEnemy);
	}

    private void Start()
    {
        NoteChangeSignal.Dispatch(NOTETYPE.MELODY);
        InvokeRepeating("Beat", 60/Beatz.beatsManager.bpm, 60 / Beatz.beatsManager.bpm);

        rout = ResetMovement(0);

        //set current weapon
        curWeapon = new Guitar();       // set somehow
        // set the animator
        curWeapon.anim = View.anim;
    }

    private void Beat()
    {
        hasAttack = false;
    }

    private void Update()
    {
        if (Ins.InuptManager.GetControls(INPUTTYPE.Block) && Beatz.beatsManager.IsOnBeat())
        {
            Block();
            //HitBeatIndicator();
        }

        else if (Ins.InuptManager.GetControls(INPUTTYPE.Attack) && Beatz.beatsManager.IsOnBeat(curNoteType))
        {
            SpawnAttack();
            //HitBeatIndicator();
        }
        // move for attack
        AttackMovement();

        // changing beats
        /*if (Ins.InuptManager.GetControls(INPUTTYPE.SwitchLeft))
        {
            SetSymbol(curNoteType, false);
            curNoteType -= 1;
            if (curNoteType < 0)
            {
                curNoteType = (NOTETYPE)System.Enum.GetNames(typeof(NOTETYPE)).Length - 1;
            }
            SetSymbol(curNoteType, true);
            NoteChangeSignal.Dispatch(curNoteType);
        }
        else if (Ins.InuptManager.GetControls(INPUTTYPE.SwitchRight))
        {
            SetSymbol(curNoteType, false);
            curNoteType++;
            if((int)curNoteType > System.Enum.GetNames(typeof(NOTETYPE)).Length -1)
            {
                curNoteType = (NOTETYPE)0;
            }
            SetSymbol(curNoteType, true);
            NoteChangeSignal.Dispatch(curNoteType);
        }*/
    }

    private void HitBeatIndicator()
    {
        // to let player know they hit beat
        if(curNoteType == NOTETYPE.MELODY)
        {
            View.melodySymbol.GetComponent<Animator>().Play("HitBeat");
        }
        else if (curNoteType == NOTETYPE.BASS)
        {
            View.bassSymbol.GetComponent<Animator>().Play("HitBeat");
        }
        else if (curNoteType == NOTETYPE.SNARE)
        {
            View.snareSymbol.GetComponent<Animator>().Play("HitBeat");
        }
        //View.platformAnim.Play("HitBeat");
    }

    private void Block()
    {
        View.shieldAnim.Play("ShieldActive");
    }

    private void MoveHitDetection()
    {
        //rotate View.hitDetection game object
    }

    private void SpawnAttack()
    {
        if (!hasAttack)
        {
            
            hasAttack = true;

            if(curNoteType == NOTETYPE.BASS)
            {
                StartCoroutine(DelayedAttack(atkMoveTime, View.bassAtk));
                curWeapon.BassAttack();
                StopMovementSignal.Dispatch(true);
                StartCoroutine(ResetMovement(.5f));
                // reset couroutine so timing isnt off
                StopCoroutine(rout);
            }
            else if (curNoteType == NOTETYPE.SNARE)
            {
                GameObject go = Instantiate(View.snareAtk, transform.position, Quaternion.Euler(new Vector3(-90,transform.rotation.eulerAngles.y + 180,0)));
                Destroy(go, 2);
                curWeapon.SnareAttack();
                StopMovementSignal.Dispatch(true);
                StartCoroutine(ResetMovement(.1f));
                StopCoroutine(rout);
            }
            else if (curNoteType == NOTETYPE.MELODY)
            {
                GameObject d = Instantiate(View.projectile, transform.position, Quaternion.Euler(transform.rotation.eulerAngles));
                curWeapon.MelodyAttack();
            }

            //reset atk movement timer
            atkTimer = 0;
            //transform.position += Vector3.forward * curWeapon.AttackDistance();

        }
        //d.transform.localPosition = Vector3.zero;
        //d.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    private void SetDistanceFromEnemy(Vector3 _enemyPos)
    {
        distFromEnemy = (_enemyPos - transform.position).magnitude;
        enemyPos = _enemyPos;
    }

    private void AttackMovement()
    {
        if(atkTimer < atkMoveTime)
        {
            float percent = Time.deltaTime / atkMoveTime;
            transform.position += View.model.transform.forward * (curWeapon.AttackDistance() * percent);
            atkTimer += Time.deltaTime;
        }
    }

    private void ChangeNoteType(NOTETYPE n)
    {
        curNoteType = n;
    }

    private void SetSymbol(NOTETYPE type, bool b)
    {
        if (type == NOTETYPE.MELODY)
        {
            View.melodySymbol.SetActive(b);
        }
        else if (type == NOTETYPE.BASS)
        {
            View.bassSymbol.SetActive(b);
        }
        else if (type == NOTETYPE.SNARE)
        {
            View.snareSymbol.SetActive(b);
        }
    }

    private IEnumerator ResetMovement(float time)
    {
        // set timer for attack timer (enabling movement again)
        yield return new WaitForSeconds(time);
        StopMovementSignal.Dispatch(false);
    }

    private IEnumerator DelayedAttack(float secs, GameObject g)
    {
        yield return new WaitForSeconds(secs);
        GameObject go =  Instantiate(g, transform.position, Quaternion.Euler(new Vector3(-90,0,0)));
        Destroy(go, 2);
    }
}
