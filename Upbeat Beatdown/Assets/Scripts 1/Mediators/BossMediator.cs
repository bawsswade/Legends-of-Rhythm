using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using System;

public class BossMediator : Mediator {

    [Inject] public BossView View { get; set; }
    [Inject] public OnBassAttackSignal BassAtkSignal { get; set; }
    [Inject] public OnMelodyAttackSignal MelodyAtkSignal { get; set; }
    [Inject] public OnBossTakeDamage BossDamageSignal { get; set; }
    [Inject] public OnInstantAttackSignal InstantAtkSignal { get; set; }

    private float startHealth;
    private GameObject player;

	public override void OnRegister()
    {
        BossDamageSignal.AddListener(TakeDamage);
        BassAtkSignal.AddListener(SpawnAOE);
        MelodyAtkSignal.AddListener(SpawnGround);
        InstantAtkSignal.AddListener(SpawnInstant);
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        startHealth = View.health;
    }

    public void SpawnGround()
    {
        Vector3 v = new Vector3(0, UnityEngine.Random.Range(0.0f, 360.0f), 0);

        Vector3 p = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
        GameObject g = Instantiate(View.groundAtk, p, Quaternion.Euler(v)) as GameObject;
        //Destroy(g, 3f);
    }

    public void SpawnInstant()
    {
        Vector3 p = new Vector3(player.transform.position.x + UnityEngine.Random.Range(-5, 5), -1.2f, player.transform.position.z + UnityEngine.Random.Range(-5, 5));
        GameObject g = Instantiate(View.instantAOE, p, Quaternion.Euler(90,0,0)) as GameObject;
        Destroy(g, 2f);
    }

    public void SpawnAOE()
    {
        //Vector3 v = new Vector3(-90.0f, 0, Random.Range(0.0f, 360.0f));
        Vector3 p = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
        GameObject g = Instantiate(View.aoeAtk, p, Quaternion.Euler(Vector3.zero)) as GameObject;
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
}
