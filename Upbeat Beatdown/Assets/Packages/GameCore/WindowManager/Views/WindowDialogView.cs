using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using UnityEngine.UI;

public class WindowDialogView : View
{
    public GameObject Modal;
    public Text TextHeader;
    public Text TextMessageBody;
    public Button ButtonDismiss;
    public Slider Slider;
    public float CloseAfter;
    public WindowData WindowData = null;
    [HideInInspector]
    public WindowManager WindowManager;
    private Coroutine Stop = null;

    public void Initialize()
    {
        if (null != WindowData)
        {
            if (null != TextHeader)
            {
                TextHeader.text = WindowData.HeaderText;
            }

            if (null != TextMessageBody)
            {
                TextMessageBody.text = WindowData.MessageBody;
            }
            if (null != Slider)
            {
                Slider.value = WindowData.SliderValue;
                if (null != WindowData.SliderFullCallback)
                {
                    Slider.onValueChanged.RemoveListener(CheckSliderFull);
                    Slider.onValueChanged.AddListener(CheckSliderFull);
                }
            }
        }

        if (null != ButtonDismiss)
        {
            ButtonDismiss.enabled = true;
            ButtonDismiss.onClick.RemoveListener(CloseWindow);
            ButtonDismiss.onClick.AddListener(CloseWindow);
        }

        if (null != Modal)
        {
            Modal.SetActive(true);
        }

        if (CloseAfter > 0.0)
        {
            if (null != Stop)
            {
                StopCoroutine(Stop);
            }
            Stop = StartCoroutine(CloseAfterTime());
        }
    }

    public void OnRemove()
    {
        Debug.Log("Removing stuff");
        if (null != WindowData)
        {
            if (null != Slider)
            {
                if (null != WindowData.SliderFullCallback)
                {
                    Slider.onValueChanged.RemoveListener(CheckSliderFull);
                }
            }
        }

        if (null != ButtonDismiss)
        {
            ButtonDismiss.enabled = false;
            ButtonDismiss.onClick.RemoveListener(CloseWindow);
        }

        if (null != Modal)
        {
            Modal.SetActive(false);
        }

        if (CloseAfter > 0.0)
        {
            if (null != Stop)
            {
                StopCoroutine(Stop);
                Stop = null;
            }
        }
    }

    public void OnRegister(WindowManager aWindowManager)
    {
        WindowManager = aWindowManager;
        Initialize();
    }
    private IEnumerator CloseAfterTime()
    {
        float timeToClose = Time.time + CloseAfter;
        while (timeToClose > Time.time)
        {
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("CloseAfterTime fired...");
        WindowManager.WindowHide(gameObject.name);
        if (null != WindowData && null != WindowData.CloseAfterCallback)
        {
            Debug.Log("CloseAfter Invoked.");
            WindowData.CloseAfterCallback.Invoke();
        }
    }

    private void CheckSliderFull(float aValue)
    {
        if(aValue >= Slider.maxValue)
        {
            WindowData.SliderFullCallback.Invoke();
        }
    }

    private void CloseWindow()
    {
        WindowManager.WindowHide(gameObject.name);
        if (null != WindowData)
        {
            if (null != WindowData.ButtonDismissCallback)
            {
                WindowData.ButtonDismissCallback();
            }
        } 
    }
}
