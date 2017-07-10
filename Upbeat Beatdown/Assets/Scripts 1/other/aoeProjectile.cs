using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aoeProjectile : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerInputMediator>().MissedNote(other.transform);
            Destroy(gameObject);
        }
        else if (other.tag == "hit")
        {
            Destroy(gameObject);
        }
    }
}
