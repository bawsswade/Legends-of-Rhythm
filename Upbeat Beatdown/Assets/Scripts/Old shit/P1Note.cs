using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Beatz;

public class P1Note : MonoBehaviour {

    public GameObject hitParticles;
    public NoteType noteType;

    public void HasHitNote()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        if (hitParticles != null)
        {
            hitParticles.SetActive(true);
        }
        Destroy(gameObject, 1);
    }
}
