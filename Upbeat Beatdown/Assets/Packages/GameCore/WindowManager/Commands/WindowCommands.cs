using strange.extensions.command.impl;
using strange.extensions.signal.impl;
using UnityEngine;

public class ShowWindowCommand : Command
{
    [Inject] public string WindowName { get; set; }
    [Inject] public WindowManager WindowManager { get; set; }

    public override void Execute()
    {
        WindowManager.WindowShow(WindowName);
    }
}

public class ShowWindowDialogCommand : Command
{
    [Inject] public string WindowName { get; set; }
    [Inject] public WindowData WindowData { get; set; }
    [Inject] public WindowManager WindowManager { get; set; }

    public override void Execute()
    {
        WindowManager.WindowShow(WindowName, WindowData);
    }
}

public class ShowWindowGameObjectCommand : Command
{
    [Inject] public GameObject GameObject { get; set; }
    [Inject] public WindowManager WindowManager { get; set; }

    public override void Execute()
    {
        WindowManager.WindowShow(GameObject);
    }
}

public class HideWindowCommand : Command
{
    [Inject] public string WindowName { get; set; }
    [Inject] public WindowManager WindowManager { get; set; }

    public override void Execute()
    {
        WindowManager.WindowHide(WindowName);
    }
}

public class SliderValueSetCommand : Command
{
    [Inject] public string WindowName { get; set; }
    [Inject] public float Value { get; set; }
    [Inject] public WindowManager WindowManager { get; set; }

    public override void Execute()
    {
        WindowManager.SliderValueSet(WindowName, Value);
    }
}

public class SliderValueAddCommand : Command
{
    [Inject] public string WindowName { get; set; }
    [Inject] public float Value { get; set; }
    [Inject] public WindowManager WindowManager { get; set; }

    public override void Execute()
    {
        WindowManager.SliderValueAdd(WindowName, Value);
    }
}

public class SliderValueSubCommand : Command
{
    [Inject] public string WindowName { get; set; }
    [Inject] public float Value { get; set; }
    [Inject] public WindowManager WindowManager { get; set; }

    public override void Execute()
    {
        WindowManager.SliderValueSub(WindowName, Value);
    }
}

public class SliderValueGetCommand :Command
{
    [Inject] public string WindowName { get; set; }
    [Inject] public WindowManager WindowManager { get; set; }
    [Inject] public SliderValueGetResponse SliderValueGetResponse { get; set; }

    public override void Execute()
    {
        float rtnValue = WindowManager.SliderValueGet(WindowName);
        SliderValueGetResponse.Dispatch(rtnValue);
    }
}