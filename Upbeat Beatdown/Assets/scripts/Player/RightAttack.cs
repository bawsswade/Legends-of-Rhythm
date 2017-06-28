using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RightAttack : MonoBehaviour {

    public UnityEvent hitNoteEvent = new UnityEvent();
    public beatsManager beatMan;
    public player_input player;
    bool hasHit = false;

    private void OnTriggerStay(Collider other)
    {
        // check if on beat
        if (other.tag == "note" && player.noteHit && hasHit == false)
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
