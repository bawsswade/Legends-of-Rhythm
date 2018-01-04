using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1Note : MonoBehaviour {

    public GameObject hitParticles;

    public void HasHitNote()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        hitParticles.SetActive(true);
        Destroy(gameObject, 1);
    }
}
