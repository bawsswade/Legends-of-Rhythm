using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using System;

public class BossMediator : Mediator {

    [Inject] public BossView View { get; set; }
    [Inject] public OnBassAttackSignal BassAtkSignal { get; set; }
    [Inject] public OnMelodyAttackSignal MelodyAtkSignal { get; set; }

	public override void OnRegister()
    {

	}

    private void Start()
    {
        BassAtkSignal.AddListener(SpawnAOE);
        MelodyAtkSignal.AddListener(SpawnGround);
    }

    public void SpawnGround()
    {
        Vector3 v = new Vector3(0, UnityEngine.Random.Range(0.0f, 360.0f), 0);

        Vector3 p = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
        GameObject g = Instantiate(View.groundAtk, p, Quaternion.Euler(v)) as GameObject;
        //Destroy(g, 3f);
    }

    public void SpawnAir()
    {
        Vector3 v = new Vector3(UnityEngine.Random.Range(0.0f, 180.0f), 0, UnityEngine.Random.Range(0.0f, 360.0f));
        Vector3 p = new Vector3(gameObject.transform.position.x, 3.0f, gameObject.transform.position.z);
        GameObject g = Instantiate(View.airAtk, p, Quaternion.Euler(v)) as GameObject;
        Destroy(g, 15f);
    }

    public void SpawnAOE()
    {
        //Vector3 v = new Vector3(-90.0f, 0, Random.Range(0.0f, 360.0f));
        Vector3 p = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
        GameObject g = Instantiate(View.aoeAtk, p, Quaternion.Euler(Vector3.zero)) as GameObject;
        Destroy(g, 5f);
    }
}
