using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;

public class PlayerMovementView : View {
    public GameObject model;
    public GameObject shield;
    public GameObject noteIndicators;

    public GameObject lockedTarget;
    public GameObject camPivot;
    public GameObject camera;

    public bool isLockedOn;
    public Animator anim;

    public GameObject DashParts;
}
