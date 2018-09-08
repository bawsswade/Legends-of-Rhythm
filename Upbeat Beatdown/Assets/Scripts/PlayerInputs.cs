using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour {
	public KeyCode MoveLeft;
	public KeyCode MoveRight;

	private void Update()
	{
		if (Input.GetKeyDown(MoveLeft))
		{
			transform.position = new Vector3(transform.position.x -2, transform.position.y, transform.position.z);
		}
		if (Input.GetKeyDown(MoveRight))
		{
			transform.position = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z);
		}
	}
}
