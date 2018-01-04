using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Beatz;

public class Rotate : MonoBehaviour {

    public bool isRandom;
    public float rate;

    public int beatLifetime;
    private SongManager sm;
    private int count = 0;

    private void Start()
    {
        sm = FindObjectOfType<SongManager>();
        InvokeRepeating("Increment", 0, 60f / sm.bpm);
        /*if (isRandom)
        {
            rate = Random.Range(.1f, 1);
        }*/
    }

    private void Update()
    {
        gameObject.transform.Rotate(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)));
    }

    private void Increment()
    {
        count++;
        if (count > beatLifetime)
        {
            Destroy(gameObject);
        }
    }
}
