using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;

public class PlayerActionsView : View {
    public GameObject shield;
    public GameObject model;

    public GameObject projectile;
    public GameObject bassAtk;
    public GameObject snareAtk;

    public Animator platformAnim;
    public Animator shieldAnim;

    public GameObject melodySymbol;
    public GameObject bassSymbol;
    public GameObject snareSymbol;

    public Animator anim;

    public BoxCollider aimDetection;
}
