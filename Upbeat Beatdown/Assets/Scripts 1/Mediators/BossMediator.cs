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
        if (Beatz.beatsManager.IsOnBeat(NOTETYPE.MELODY) && curNoteType != NOTETYPE.MELODY && !melodyHasSpawned)
        {
            SpawnGround();
            melodyHasSpawned = true;
        }
        if (Beatz.beatsManager.IsOnBeat(NOTETYPE.BASS) && curNoteType != NOTETYPE.BASS && !bassHasSpawned)
        {
            SpawnAOE();
            bassHasSpawned = true;
        }
        if (Beatz.beatsManager.IsOnBeat(NOTETYPE.SNARE) && curNoteType != NOTETYPE.SNARE && !snareHasSpawned)
        {
            SpawnInstantLine();
            snareHasSpawned = true;
        }

        ResetVars();
    }

    public void Spawn4Ground()
    {
        Vector3 v = new Vector3(0, UnityEngine.Random.Range(0.0f, 360.0f), 0);
        StartCoroutine(SpawnMultGround(4, v));
    }

    IEnumerator SpawnMultGround(int numTimes, Vector3 v)
    {
        int count = 0;
        while (count < numTimes)
        {
            Vector3 p = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
            GameObject g = Instantiate(View.groundAtk, p, Quaternion.Euler(v), View.melodyBossAtks.transform) as GameObject;
            count++;
            yield return new WaitForSeconds(.42f);
        }
    }

    public void SpawnCircle()
    {
        float increment = 0;
        float num = 8;
        for (int i = 0; i < num; i++)
        {
            Vector3 v = new Vector3(0,  increment, 0);
            Vector3 p = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
            GameObject g = Instantiate(View.groundAtk, p, Quaternion.Euler(v), View.melodyBossAtks.transform) as GameObject;
            increment += 360/num;
        }
    }

    public void SpawnGround()
    {
        Vector3 v = new Vector3(0, UnityEngine.Random.Range(0.0f, 360.0f), 0);

        Vector3 p = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
        GameObject g = Instantiate(View.groundAtk, p, Quaternion.Euler(v), View.melodyBossAtks.transform) as GameObject;
        //Destroy(g, 3f);
    }

    public void SpawnArcGround()
    {
        Vector3 v = new Vector3(0, UnityEngine.Random.Range(0.0f, 360.0f), 0);
        Vector3 p = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
        Instantiate(View.groundAtk, p, Quaternion.Euler(v), View.melodyBossAtks.transform);
        Instantiate(View.groundAtk, p, Quaternion.Euler(0,v.y - 15, 0), View.melodyBossAtks.transform);
        Instantiate(View.groundAtk, p, Quaternion.Euler(0, v.y + 15, 0), View.melodyBossAtks.transform);
    }

    public void SpawnInstant()
    {
        Vector3 p = new Vector3(player.transform.position.x + UnityEngine.Random.Range(-5, 5), -1.2f, player.transform.position.z + UnityEngine.Random.Range(-5, 5));
        GameObject g = Instantiate(View.instantAOE, p, Quaternion.Euler(90,0,0)) as GameObject;     // set parent transform
        Destroy(g, 2f);
    }

    public void SpawnInstantLine()
    {
        Vector3 p = new Vector3(player.transform.position.x + UnityEngine.Random.Range(-5, 5), -1.2f, player.transform.position.z + UnityEngine.Random.Range(-5, 5));
        GameObject g = Instantiate(View.instantLine, Vector3.zero, Quaternion.LookRotation(new Vector3( p.x,0,p.z)), View.snareBossAtks.transform) as GameObject;
        Destroy(g, 2f);
    }

    public void SpawnAOE()
    {
        //Vector3 v = new Vector3(-90.0f, 0, Random.Range(0.0f, 360.0f));
        Vector3 p = new Vector3(transform.position.x, 0, transform.position.z);
        GameObject g = Instantiate(View.aoeAtk, Vector3.zero, Quaternion.Euler(p), View.bassBossAtks.transform) as GameObject;
        Destroy(g, 5f);
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
