using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class WindowDialogMediator : Mediator
{
    [Inject] public WindowDialogView View { get; set; }
    [Inject] public WindowManager WindowManager { get; set; }

    public override void OnRegister()
    {
        View.OnRegister(WindowManager);
    }

    public override void OnRemove()
    {
        if (View == null) { return; }
        View.OnRemove();
    }
}
