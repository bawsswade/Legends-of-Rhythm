using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using System;

public class RightAttackMediator : Mediator {

    [Inject] public RightAttackView View { get; set; }
    [Inject] public OnRightHit RightHitSignal { get; set; }
    [Inject] public OnChargeSpecial ChargeSpecialSignal { get; set; }
    [Inject] public OnRightResetHit ResetRightSignal { get; set; }
    [Inject] public OnGainHealth GainHealthSignal { get; set; }

    public bool hasHit = false;

    public override void OnRegister()
    {
        ResetRightSignal.AddListener(ResetHit);
    }

    private void Update()
    {
        hasHit = false;
    }

    private void OnTriggerStay(Collider other)
    {
        // check if on beat
        if (other.tag == "note" && View.player.GetComponent<PlayerInputView>().noteHit && hasHit == false /*&& !other.GetComponent<projectileSeek>().isDeflected*/)
        {
            // spawns hit particles 
            RightHitSignal.Dispatch();  
            //hitNoteEvent.Invoke();
            hasHit = true;
            GainHealthSignal.Dispatch(1);
        }
        else
        {
            //hasHit = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (View.player.GetComponent<PlayerInputView>().isDashing && other.tag == "note")
        {
            // increase charge
            ChargeSpecialSignal.Dispatch();
        }
    }

    private void ResetHit()
    {
        hasHit = false;
    }
}
