using UnityEngine;
using System.Collections;

public class WindowDemoScript : MonoBehaviour
{
    public WindowView View;
    private WindowManager _windowManager;
    void Awake()
    {
        _windowManager = FindObjectOfType<WindowManager>();
        _windowManager.Windows.Add(View);
    }

    public void ShowWindow()
    {
        Debug.Log("Button Clicked");

        //NOTE This can be done like this or by using [Inject] public ShowWindowSignal ShowWindowSignal {get; set;}   (using) ShowWindowSingal.Dispatch("Window");
        //NOTE This can also be done by using [Inject] public WindowManager WindowManger {get; set;}  (using) WindowManager.WindowShow("Window");
        _windowManager.WindowShow("Window");
    }

    public void HideWindow()
    {
        //NOTE This can be done like this or by using [Inject] HideWindowSignal HideWindowSignal {get; set;}   (using) HideWindowSingal.Dispatch("Window");
        //NOTE This can also be done by using [Inject] public WindowManager WindowManger {get; set;}  (using) WindowManager.WindowHide("Window");
        _windowManager.WindowHide("Window");
    }
}
