using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Beatz;

public class Expand : MonoBehaviour {

    public bool isRandom;
    public float rate;

    public int beatLifetime;
    private SongManager sm;
    private int count = 0;

    private void Start()
    {
        sm = FindObjectOfType<SongManager>();
        InvokeRepeating("Increment", 0, 60f/sm.bpm);
        if(isRandom)
        {
            rate = Random.Range(0,1);
        }
    }

    private void Update()
    {
        gameObject.transform.localScale += new Vector3(rate, rate, rate);
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
