using UnityEngine;
using System.Collections;

public class player_motor : MonoBehaviour {
    private Rigidbody r;

    private Vector3 m_force;
    private Vector3 j_force;
    private Vector3 _rotation;
	
	void Start () {
        r = gameObject.GetComponent<Rigidbody>();
	}
	
	void Update () {
        Move();
        Rotate();
	}

    public void setForce(Vector3 forceToAdd)
    {
        m_force = forceToAdd;
    }

    public void setRot(Vector3 rotToAdd)
    {
        _rotation = rotToAdd;
    }

    public void setJump(Vector3 forceToAdd)
    {
        j_force = forceToAdd;
    }

    void Move()
    {
        r.AddForce(m_force);
        if (j_force != Vector3.zero)
        {
            r.AddForce(j_force, ForceMode.Acceleration);
        }
    }

    void Rotate()
    {
        r.MoveRotation(r.rotation * Quaternion.Euler(_rotation));
    }

}
