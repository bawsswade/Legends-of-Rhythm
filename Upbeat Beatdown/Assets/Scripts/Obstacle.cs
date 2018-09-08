using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// increases in size for now
public class Obstacle : MonoBehaviour {

	// change color?
	private void OnIncrement()
	{

	}

	private void OnBeat()
	{
		transform.localScale = new Vector3(transform.localScale.x * 3f, transform.localScale.y, transform.localScale.z);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			OnBeat();
		}
	}
}
