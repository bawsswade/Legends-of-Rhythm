using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using System;

public class LeftAttackMediator : Mediator {

    [Inject] public LeftAttackView View { get; set; }
    [Inject] public OnLeftHit LeftHitSignal { get; set; }
    [Inject] public OnChargeSpecial ChargeSpecialSignal { get; set; }
    [Inject] public OnLeftResetHit ResetLeftSignal { get; set; }
    [Inject] public OnGainHealth GainHealthSignal { get; set; }

    public bool hasHit = false;

    public override void OnRegister()
    {
        ResetLeftSignal.AddListener(ResetHit);
    }

    private void Update()
    {
        hasHit = false;
    }

    private void OnTriggerStay(Collider other)
    {
        // check if on beat 
        if (other.tag == "note" && View.player.GetComponent<PlayerInputView>().noteHit && hasHit == false /*&& !other.GetComponent<projectileSeek>().isDeflected*/)     // make so player cant hit note after deflected
        {
            // spawns hit particles 
            LeftHitSignal.Dispatch();
            //hitNoteEvent.Invoke();
            hasHit = true;

            // increase health if can
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
