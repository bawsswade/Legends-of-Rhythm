using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using System;
using System.Collections.Generic;

public class EnvBuildMediator : Mediator {
    [Inject] public EnvBuildView View { get; set; }

    private int numOrbs = 15;
    private List<Orb> OrbList = new List<Orb>();

	public override void OnRegister()
    {

	}

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        for (int i = 0; i < numOrbs; i++)
        {
            Vector3 v = new Vector3(UnityEngine.Random.Range(-25, 25), 0, UnityEngine.Random.Range(-25, 25));
            GameObject g = Instantiate(View.orb, v + player.transform.position, Quaternion.identity);
            OrbList.Add(g.GetComponent<Orb>());
        }

        InvokeRepeating("ActivateOrbs",0, Beatz.beatsManager.secPerBeat * 4);
    }

    private void ActivateOrbs()
    {
        // get random orb and activate
        if (OrbList.Count > 0)
        {
            int i = UnityEngine.Random.Range(0, OrbList.Count - 1);
            OrbList[i].Activate();
            OrbList.Remove(OrbList[i]);
        }
    } 
}
