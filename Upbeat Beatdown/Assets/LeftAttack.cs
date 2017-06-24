using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class LeftAttack : MonoBehaviour {

    public UnityEvent hitNoteEvent = new UnityEvent();

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "note")
        {
            hitNoteEvent.Invoke();
        }
    }
}
