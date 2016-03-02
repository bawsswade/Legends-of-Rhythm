using UnityEngine;
using System.Collections;

public class weaponList : MonoBehaviour {

    public GameObject[] wList;

	public GameObject FindWeapon(string name)
    {
        foreach(GameObject g in wList)
        {
            if(g.name == name)
            {
                return g;
            }
        }
        Debug.Log("weapon not found");
        return null;
    }
}
