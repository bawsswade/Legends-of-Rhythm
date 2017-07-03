using strange.extensions.signal.impl;
using UnityEngine;

public class ShowWindowSignal : Signal<string> { }
public class HideWindowSignal : Signal<string> { }
public class ShowWindowDialogSignal : Signal<string, WindowData> { }
public class ShowWindowGameObjectSignal : Signal<GameObject> { }
public class ShowWindowResponse : Signal<GameObject> { }
public class SliderValueGetSignal : Signal<string> { }
public class SliderValueSetSignal : Signal<string, float> { }
public class SliderValueAddSignal : Signal<string, float> { }
public class SliderValueSubSignal : Signal<string, float> { }
public class SliderValueGetResponse : Signal<float> { }