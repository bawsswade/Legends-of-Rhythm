using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Beatz;

public class Translate : MonoBehaviour {
    public float rate;

    public Vector3 dirVec;
    public Vector3 endVec;

    public int beatLifetime;
    //private SongManager sm;
    private int count = 0;

    public Translate(Vector3 _dirVec, Vector3 _endVec, float _rate, int _beatLifetime = 0)
    {
        dirVec = _dirVec;
        endVec = _endVec;
        rate = _rate;
        beatLifetime = _beatLifetime;
    }

    private void Start()
    {
        if (beatLifetime != 0)
        {
            InvokeRepeating("Increment", 0, 60f / SongManager.bpm);
        }
    }

    private void Update()
    {

        gameObject.transform.position += dirVec;
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
