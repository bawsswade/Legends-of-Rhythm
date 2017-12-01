using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour {
    public GameObject particles;
    

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            GameObject g = Instantiate(particles, gameObject.transform.position, Quaternion.identity);
            Destroy(g, 1);
            Destroy(gameObject);
        }
    }

    public void Activate()
    {
        // go through animation

        // change material with beat

        // when done
        OnTriggered();
    }

    private void OnTriggered()
    {
        // activate collider
        GetComponent<SphereCollider>().enabled = true;

        // play explosion animation
        GetComponent<Animator>().SetTrigger("activate");

        // destroy()
        Destroy(gameObject, 2);
    }
}
