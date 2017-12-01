using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using System;

public class PlayerActionsMediator : Mediator {

    [Inject] public PlayerActionsView View { get; set; }
    [Inject] public OnChangeNoteType NoteChangeSignal { get; set; }
<<<<<<< HEAD
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
=======


    public NOTETYPE curNoteType;
>>>>>>> ae8663e27890825f99da0550b67eec12000e619c

	public override void OnRegister()
    {
        NoteChangeSignal.AddListener(ChangeNoteType);
<<<<<<< HEAD
        EnemyInRangeSignal.AddListener(SetDistanceFromEnemy);
=======
>>>>>>> ae8663e27890825f99da0550b67eec12000e619c
	}

    private void Start()
    {
        NoteChangeSignal.Dispatch(NOTETYPE.MELODY);
<<<<<<< HEAD
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
=======
>>>>>>> ae8663e27890825f99da0550b67eec12000e619c
    }

    private void Update()
    {
        if (Ins.InuptManager.GetControls(INPUTTYPE.Block) && Beatz.beatsManager.IsOnBeat())
        {
            Block();
<<<<<<< HEAD
            //HitBeatIndicator();
=======
            HitBeatIndicator();
>>>>>>> ae8663e27890825f99da0550b67eec12000e619c
        }

        else if (Ins.InuptManager.GetControls(INPUTTYPE.Attack) && Beatz.beatsManager.IsOnBeat(curNoteType))
        {
            SpawnAttack();
<<<<<<< HEAD
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
=======
            HitBeatIndicator();
        }

        // changing beats
        if (Ins.InuptManager.GetControls(INPUTTYPE.SwitchLeft))
        {
            curNoteType -= 1;
>>>>>>> ae8663e27890825f99da0550b67eec12000e619c
            NoteChangeSignal.Dispatch(curNoteType);
        }
        else if (Ins.InuptManager.GetControls(INPUTTYPE.SwitchRight))
        {
<<<<<<< HEAD
            SetSymbol(curNoteType, false);
            curNoteType++;
            if((int)curNoteType > System.Enum.GetNames(typeof(NOTETYPE)).Length -1)
            {
                curNoteType = (NOTETYPE)0;
            }
            SetSymbol(curNoteType, true);
            NoteChangeSignal.Dispatch(curNoteType);
        }*/
=======
            curNoteType++;
            NoteChangeSignal.Dispatch(curNoteType);
        }
>>>>>>> ae8663e27890825f99da0550b67eec12000e619c
    }

    private void HitBeatIndicator()
    {
<<<<<<< HEAD
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
=======
        View.platformAnim.Play("HitBeat");
>>>>>>> ae8663e27890825f99da0550b67eec12000e619c
    }

    private void Block()
    {
        View.shieldAnim.Play("ShieldActive");
    }

<<<<<<< HEAD
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
=======
    private void SpawnAttack()
    {
        GameObject d = Instantiate(View.projectile, transform.position ,Quaternion.Euler(0,180,0));
>>>>>>> ae8663e27890825f99da0550b67eec12000e619c
        //d.transform.localPosition = Vector3.zero;
        //d.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

<<<<<<< HEAD
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

=======
>>>>>>> ae8663e27890825f99da0550b67eec12000e619c
    private void ChangeNoteType(NOTETYPE n)
    {
        curNoteType = n;
    }
<<<<<<< HEAD

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
=======
>>>>>>> ae8663e27890825f99da0550b67eec12000e619c
}
