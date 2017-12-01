using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class WindowMediator : Mediator
{
    [Inject] public WindowView View { get; set; }
    [Inject] public WindowManager Manager { get; set; }
    public override void OnRegister()
    {
        Manager.Windows.Add(View);
    }
}
