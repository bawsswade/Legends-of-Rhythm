using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class bassSine : MonoBehaviour {

    private LineRenderer lr;
    public float velocity;
    public float angle;
    public int resolution;

	// Use this for initialization
	void Start () {
        lr = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
