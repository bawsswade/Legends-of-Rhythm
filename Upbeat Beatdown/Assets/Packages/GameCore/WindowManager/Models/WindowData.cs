using System;

public class WindowData
{
    public string HeaderText = null;
    public string MessageBody = null;
    public float SliderValue = 0.0f;
    // callbacks to perform for different functions
    public Action ButtonDismissCallback = null;
    public Action SliderFullCallback = null;
    public Action CloseAfterCallback = null;
}