using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using UnityEngine.Events;

public class NewBehaviourScript : MonoBehaviour {
	public string eventId;
	public UnityEvent OnEventTriggered;
	public UnityEvent OnEventEnd;

	private void Start()
	{
		Koreographer.Instance.RegisterForEvents(eventId, Fire);
	}

	void Fire(KoreographyEvent kEvent)
	{
		OnEventTriggered.Invoke();
		float halfBpm = .25f;//Koreographer.Instance.GetMusicBPM()
		Invoke("Disable", halfBpm);
	}

	private void Disable()
	{
		OnEventEnd.Invoke();
	}
}
