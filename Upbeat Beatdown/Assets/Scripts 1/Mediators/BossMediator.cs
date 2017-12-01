using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using System;

public class BossMediator : Mediator {

    [Inject] public BossView View { get; set; }
    [Inject] public OnChangeNoteType NoteChangeSignal { get; set; }
    [Inject] public OnBossTakeDamage BossDamageSignal { get; set; }
    //[Inject] public OnBassAttackSignal BassAtkSignal { get; set; }
    //[Inject] public OnMelodyAttackSignal MelodyAtkSignal { get; set; }
    //[Inject] public OnInstantAttackSignal InstantAtkSignal { get; set; }

    private float startHealth;
    private GameObject player;

    private NOTETYPE curNoteType;
    private int count = 0;
    private bool melodyHasSpawned = false;
    private bool bassHasSpawned = false;
    private bool snareHasSpawned = false;

    public override void OnRegister()
    {
        BossDamageSignal.AddListener(TakeDamage);
        NoteChangeSignal.AddListener(ChangeNoteType);
        //BassAtkSignal.AddListener(SpawnAOE);
        //MelodyAtkSignal.AddListener(SpawnGround);
        //InstantAtkSignal.AddListener(SpawnInstantLine);
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        startHealth = View.health;

        // beat increments
        float secPerBeat = 60f / Beatz.beatsManager.bpm;
        InvokeRepeating("AttackCheck", secPerBeat, secPerBeat);
    }

    private void AttackCheck()
    {
        if (Beatz.beatsManager.IsBossOnBeat(NOTETYPE.MELODY) && curNoteType != NOTETYPE.MELODY && !melodyHasSpawned)
        {
            //SpawnGround();
            View.Attacks.MelodyAtk();
            melodyHasSpawned = true;
        }
        if (Beatz.beatsManager.IsBossOnBeat(NOTETYPE.BASS) && curNoteType != NOTETYPE.BASS && !bassHasSpawned)
        {
            //SpawnAOE();
            View.Attacks.BassAtk();
            bassHasSpawned = true;
        }
        if (Beatz.beatsManager.IsBossOnBeat(NOTETYPE.SNARE) && curNoteType != NOTETYPE.SNARE && !snareHasSpawned)
        {
            //SpawnInstantLine();
            View.Attacks.SnareAtk();
            snareHasSpawned = true;
        }

        ResetVars();
    }
    

    

    public void TakeDamage(int damage)
    {
        if (View.health > damage)
        {
            View.health -= damage;
            View.healthDisplay.transform.localScale = new Vector3(View.health / startHealth, 1, 1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "playerAttack")
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    private void ChangeNoteType(NOTETYPE n)
    {
        curNoteType = n;
    }

    private void ResetVars()
    {
        melodyHasSpawned = false;
        bassHasSpawned = false;
        snareHasSpawned = false;
    }
}
