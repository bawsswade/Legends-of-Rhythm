using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RightAttack : MonoBehaviour {

    public UnityEvent hitNoteEvent = new UnityEvent();

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "note")
        {
            hitNoteEvent.Invoke();
        }
    }
}
