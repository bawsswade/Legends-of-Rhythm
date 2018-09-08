using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Beatz;

public class Rotate :MonoBehaviour {

    public bool isRandom;
    public float rate;
    
    public Vector3 rotVec = new Vector3();

    public int beatLifetime;
    //private SongManager sm;
    private int count = 0;

    public Rotate(Vector3 _rotVec, float _rate, int _beatLifetime = 0, bool _isRandom = false)
    {
        isRandom = _isRandom;
        rotVec = _rotVec;
        beatLifetime = _beatLifetime;
        rate = _rate;
    }

    private void Start()
    {
        if (beatLifetime != 0)
        {
            InvokeRepeating("Increment", 0, 60f / SongManager.bpm);
        }
        if(isRandom)
        {
            rotVec = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
        }
    }

    private void Update()
    {
        gameObject.transform.Rotate(rotVec);
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
