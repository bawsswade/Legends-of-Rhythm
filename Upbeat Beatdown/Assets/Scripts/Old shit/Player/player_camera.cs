using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_camera : MonoBehaviour {

    public Camera camera;
    private player_abilities abilities;

    private Vector3 phase1Pos = new Vector3(0, 0, -20f);
    private  Vector3 phase2Pos = new Vector3(1.25f, 3, -1.5f);

    private void Start()
    {
        //camera.gameObject.transform.position = phase1Pos;

        abilities = GetComponent<player_abilities>();
        abilities.OnEnterNoteSync += CameraPos2;
        abilities.OnExitNoteSync += CameraPos1;
    }

    private void FollowPOI(Vector3 posToFollow)
    {
        camera.transform.LookAt(posToFollow);
    }

    private void CameraPos1()
    {
        camera.transform.localPosition = phase1Pos;
    }

    private void CameraPos2()
    {
        camera.transform.localPosition = phase2Pos;
    }
}
