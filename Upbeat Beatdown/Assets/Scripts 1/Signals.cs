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
public class OnGainHealth : Signal<int> { }
public class OnChangeNoteType : Signal<NOTETYPE> { }
<<<<<<< HEAD
public class OnEnemyInRange : Signal<Vector3> { }
=======
>>>>>>> ae8663e27890825f99da0550b67eec12000e619c

// enamy attacks
public class OnBassAttackSignal : Signal { }
public class OnMelodyAttackSignal : Signal { }
public class OnInstantAttackSignal : Signal { }
public class OnBossTakeDamage : Signal<int> { }

// beatzz
public class OnLeftHit : Signal { }
public class OnRightHit : Signal { }
<<<<<<< HEAD
public class OnAttacking : Signal<bool> { }
=======
>>>>>>> ae8663e27890825f99da0550b67eec12000e619c
//public class OnBassBeat : Signal<bool> { }
//public class OnMelodyBeat : Signal<bool> { }