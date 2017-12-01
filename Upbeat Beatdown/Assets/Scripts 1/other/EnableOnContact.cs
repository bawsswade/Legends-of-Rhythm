using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnContact : MonoBehaviour {

    private Renderer rend;
    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        rend.enabled = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        rend.enabled = false;
    }
}
