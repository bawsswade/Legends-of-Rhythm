using UnityEngine;
using System.Collections.Generic;
using strange.extensions.command.api;
using strange.extensions.injector.api;
using strange.extensions.mediation.api;
using System;
using strange.extensions.signal.impl;
using UnityEngine.UI;

public class WindowManager : StrangePackage
{
    public List<WindowView> Windows;
    private List<GameObject> _Windows = new List<GameObject>();

    public override void MapBindings(ICommandBinder commandBinder, ICrossContextInjectionBinder injectionBinder, IMediationBinder mediationBinder)
    {
        // bind this Object to the WindowService
        injectionBinder.Bind<WindowManager>().ToValue(this).ToSingleton();

        // bind signals and views to the main context Binders
        mediationBinder.Bind<WindowView>().To<WindowMediator>();
        mediationBinder.Bind<WindowDialogView>().To<WindowDialogMediator>();

        commandBinder.Bind<ShowWindowSignal>().To<ShowWindowCommand>();
        commandBinder.Bind<HideWindowSignal>().To<HideWindowCommand>();
        commandBinder.Bind<ShowWindowDialogSignal>().To<ShowWindowDialogCommand>();
        commandBinder.Bind<ShowWindowGameObjectSignal>().To<ShowWindowGameObjectCommand>();
        commandBinder.Bind<SliderValueGetSignal>().To<SliderValueGetCommand>();
        commandBinder.Bind<SliderValueSetSignal>().To<SliderValueSetCommand>();
        commandBinder.Bind<SliderValueAddSignal>().To<SliderValueAddCommand>();
        commandBinder.Bind<SliderValueSubSignal>().To<SliderValueSubCommand>();

        commandBinder.Bind<SliderValueGetResponse>();
    }

    public GameObject WindowShow(string name)
    {
        WindowView windowView = GetWindowView(name, true);
        GameObject window = null;
        if (null != windowView)
        {
            window = OpenWindow(windowView);
        }
        return window;
    }

    public GameObject WindowShow(GameObject gameObject)
    {
        var name = gameObject.name;
        WindowView windowView = GetWindowView(name, true);
        GameObject window = null;
        if (null != windowView)
        {
            window = OpenWindow(windowView);
        }
        return window;
    }

    public GameObject WindowShow(string name, WindowData winData)
    {

        WindowView windowView = GetWindowView(name, true);
        GameObject window = null;
        if (null != windowView)
        {
            window = OpenWindow(windowView);

            WindowDialogView windowDialogView = window.GetComponent<WindowDialogView>();
            if (null != windowDialogView)
            {
                windowDialogView.WindowData = winData;

                // re-init the view if the mediator already exists
                WindowDialogMediator windowDialogMediator = window.GetComponent<WindowDialogMediator>();
                if (null != windowDialogMediator)
                {
                    windowDialogView.Initialize();
                }
            }
        }
        return window;
    }

    public void WindowHide(string aName)
    {
        WindowView windowView = GetWindowView(aName, false);
        if (null != windowView)
        {
            CloseWindow(windowView);
        }
    }

    private void CloseWindow(WindowView windowView)
    {
        GameObject window = windowView.gameObject;
        if (windowView.DestroyOnClose)
        {
            _Windows.Remove(window);
            GameObject.Destroy(windowView.gameObject);
        }
        else
        {
            // set the animator bool 'Open' to false(let animator handle closing the window)
            Animator animator = window.GetComponent<Animator>();
            if (null != animator)
            {
                animator.SetBool("Open", false);
            }
            else
            {
                WindowDialogView view = window.GetComponent<WindowDialogView>();
                if (null != view)
                {
                    view.OnRemove();
                }
                // hide the inner window without destroying it
                if (null != windowView.InnerContent)
                {
                    windowView.InnerContent.SetActive(false);
                }
                else
                {
                    window.SetActive(false);
                }
            }
        }
    }

    private GameObject OpenWindow(WindowView windowView)
    {
        GameObject window = windowView.gameObject;
        window.transform.SetSiblingIndex(windowView.Index);

        // let animator handle opening view if it exists
        var animator = window.GetComponent<Animator>();
        if (null != animator)
        {
            animator.SetBool("Open", true);
        }
        else
        {
            foreach (var item in windowView.ItemsToShow)
            {
                item.SetActive(true);
            }

            foreach (var item in windowView.ItemsToHide)
            {
                item.SetActive(false);
            }
            if (null != windowView.InnerContent)
            {
                windowView.InnerContent.SetActive(true);
            }
            window.gameObject.SetActive(true);
        }
        return window;
    }

    private WindowView GetWindowView(string aName, bool create)
    {
        // find windowView in the window list
        WindowView windowView = Windows.Find(w => w != null && w.name == aName);
        if (windowView == null)
        {
            Debug.LogWarning("The WindowView for "+aName+" needs to be added to the Windows List.");
            return null;
        }
        // check the list of cached gameObjects
        GameObject window = _Windows.Find(w => w != null && w.name == aName);
        if (null == window)
        {
            // last ditch effort, check scene objects
            window = GameObject.Find(aName);
            if (window == null)
            {
                if (create)
                {
                    // make a new one
                    window = Instantiate(windowView.gameObject) as GameObject;
                    window.name = aName;
                    window.transform.SetParent(GameObject.Find("Canvas").transform, false);
                    window.transform.localScale = Vector3.one;
                    window.transform.localPosition = Vector3.zero;
                    // cache it for future
                    _Windows.Add(window);
                }
                else
                {
                    return null;
                }
            }
        }
        windowView = window.GetComponent<WindowView>();
        return windowView;
    }

    public float SliderValueGet(string aName)
    {
        // find the window prefab in the list of available window prefabs
        WindowView windowView = GetWindowView(aName, false);
        if (null == windowView) { Debug.LogWarning(aName + " WindowView is null."); return -1; }

        WindowDialogView windowDialogView = windowView.gameObject.GetComponent<WindowDialogView>();
        if(null == windowDialogView) { Debug.LogWarning(aName + " WindowDialogView is null."); return -1; }
        
        Slider slider = windowDialogView.Slider;
        if (null == slider) { Debug.LogWarning(aName + " WindowDialogView.Slider is null."); return -1; }
        return slider.value;
    }

    public void SliderValueSet(string aName, float aValue)
    {
        WindowView windowView = GetWindowView(aName, false);
        if (null == windowView) { Debug.LogWarning(aName + " WindowView is null."); return; }

        WindowDialogView windowDialogView = windowView.gameObject.GetComponent<WindowDialogView>();
        if (null == windowDialogView) { Debug.LogWarning(aName + " WindowDialogView is null."); return; }

        Slider slider = windowDialogView.Slider;
        if (null == slider) { Debug.LogWarning(aName + " WindowDialogView.Slider is null."); return; }
        slider.value = aValue;
    }

    public void SliderValueAdd(string aName, float aValue)
    {
        WindowView windowView = GetWindowView(aName, false);
        if (null == windowView) { Debug.LogWarning(aName + " WindowView is null."); return; }

        WindowDialogView windowDialogView = windowView.gameObject.GetComponent<WindowDialogView>();
        if (null == windowDialogView) { Debug.LogWarning(aName + " WindowDialogView is null."); return; }

        Slider slider = windowDialogView.Slider;
        if (null == slider) { Debug.LogWarning(aName + " WindowDialogView.Slider is null."); return; }
        slider.value += aValue;
    }

    public void SliderValueSub(string aName, float aValue)
    {
        WindowView windowView = GetWindowView(aName, false);
        if (null == windowView) { Debug.LogWarning(aName + " WindowView is null."); return; }

        WindowDialogView windowDialogView = windowView.gameObject.GetComponent<WindowDialogView>();
        if (null == windowDialogView) { Debug.LogWarning(aName + " WindowDialogView is null."); return; }

        Slider slider = windowDialogView.Slider;
        if (null == slider) { Debug.LogWarning(aName + " WindowDialogView.Slider is null."); return; }
        slider.value -= aValue;
    }
}