using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using System;

public class PlayerMovementMediator : Mediator {

    [Inject] public PlayerMovementView View { get; set; }

    public int speed = 100;
    public int maxSpeed = 8;

    private Rigidbody rb;
    private float lastRot;

	public override void OnRegister()
    {

	}

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    private void Update()
    {
        transform.LookAt(View.lockedTarget.transform);

        ApplyForce();
        ApplyRotation();
        
    }

    private void ApplyForce()
    {
        // movement: wasd
        float v = Input.GetAxis("PS4_L_Analog_Y");
        float h = Input.GetAxis("PS4_L_Analog_X");

        Vector3 direction = new Vector3(h, 0, v);

        if (v !=0|| h != 0)
        {
            rb.AddForce(direction * speed);
            //rb.AddForce(transform.forward * speed);
            //rb.AddForce(- transform.forward * (rb.velocity.magnitude - maxSpeed));
            //rb.AddForce(-direction * (rb.velocity.magnitude - maxSpeed));
        }
        else
        {
            //rb.AddForce(transform.forward * speed);
        }
    }

    private void ApplyRotation()
    {
        // movement: wasd
        float v = Input.GetAxis("PS4_L_Analog_Y");
        float h = Input.GetAxis("PS4_L_Analog_X");

        float angle = Mathf.Atan2(h, v) * Mathf.Rad2Deg;

        if (Mathf.Abs(v) > .1f || Mathf.Abs(h) > .1f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            if(v == 1 || h == 1)
            {
                lastRot = angle;
            }
        }
        else
        {
            //transform.rotation = Quaternion.Euler(0, lastRot, 0);
        }
    }
}
