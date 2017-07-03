using strange.extensions.signal.impl;
using UnityEngine;
using System.Collections.Generic;
using System;

// player stuff
public class OnLeftAttackSignal : Signal { }
public class OnRightAttackSignal : Signal { }
public class OnDashSignal : Signal { }
public class OnChargeSpecial : Signal { }
public class OnLeftResetHit : Signal { }
public class OnRightResetHit : Signal { }

// enamy attacks
public class OnBassAttackSignal : Signal { }
public class OnMelodyAttackSignal : Signal { }

// beatzz
public class OnLeftHit : Signal { }
public class OnRightHit : Signal { }
//public class OnBassBeat : Signal<bool> { }
//public class OnMelodyBeat : Signal<bool> { }