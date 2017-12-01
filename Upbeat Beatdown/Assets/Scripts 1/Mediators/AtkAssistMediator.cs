using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using System;

public class AtkAssistMediator : Mediator {
    
    [Inject] public AtkAssistView View { get; set; }
    [Inject] public OnEnemyInRange EnemyInRangeSignal { get; set; }

	public override void OnRegister()
    {

	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "boss" || other.tag == "enemy")
        {
            Debug.Log("locked onto" + other.name);
            View.target = other.gameObject;
            View.hasTargeted = true;

            // send enemy position to player
            EnemyInRangeSignal.Dispatch(other.transform.position);
            // set signal to actions to recieve info
        }
    }

    // disengage target on exit

    // choose target by angle instead of distance
}
