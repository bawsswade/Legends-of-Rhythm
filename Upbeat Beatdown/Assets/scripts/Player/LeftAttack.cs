using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class LeftAttack : MonoBehaviour {

    public UnityEvent hitNoteEvent = new UnityEvent();
    //public beatsManager beatMan;
    public player_input player;
    bool hasHit = false;        // to instantiate hit particles only once

    private void OnTriggerStay(Collider other)
    {
        // check if on beat
        if (other.tag == "note" && player.noteHit && hasHit == false)       // change to check beat on initial button press
        {
            // spawns hit particles
            hitNoteEvent.Invoke();
            hasHit = true;
        }
        else
        {
            hasHit = false;
        }
    }
}
