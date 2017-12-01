using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using System;

public class CameraMediator : Mediator {

    [Inject] public CameraView View { get; set; }

    private bool isLockedOn = true;

	public override void OnRegister()
    {

	}

    private void Start()
    {

    }

    private void Update()
    {
        if (isLockedOn)
        {
            //transform.RotateAround(View.lockedTarget.transform.position, Vector3.up, 0);

            Vector3 angle = (View.lockedTarget.transform.position - View.player.transform.position).normalized;
            Vector3 pos = (-angle * View.cameraDist) + View.player.transform.position;
            transform.position = new Vector3(pos.x, 8, pos.z);      // do some kind of y update 


            //float ang = Mathf.Atan2(View.player.transform.position.x - transform.position.x, View.player.transform.position.y - transform.position.y) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, ang, transform.rotation.eulerAngles.z);

            //transform.rotation.SetLookRotation(View.lockedTarget.transform.position - View.player.transform.position, Vector3.up);
            transform.LookAt(View.lockedTarget.transform);
        }
    }
}
