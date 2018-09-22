using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public class TestEventSub : MonoBehaviour {

	private void Start()
	{
		Koreographer.Instance.RegisterForEvents("TestEventID", Fire);	
	}

	void Fire(KoreographyEvent kEvent)
	{
		Debug.Log("Fired");
	}
}
