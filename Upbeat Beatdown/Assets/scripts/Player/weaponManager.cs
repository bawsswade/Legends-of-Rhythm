using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// handle inputs for switching weapons and sends perk to player input
public class weaponManager : MonoBehaviour {
    
    // weapons attained
    public List<GameObject> w_inventory;
    // current weapon equipped
    public GameObject equippedWeapon;
    // list of all possible weapons
    private weaponList allWeapons;
    private int weaponIndex;

    void Start()
    {
        allWeapons = GetComponent<weaponList>();
        AddWeapon("guitar");
        AddWeapon("keytar");

        equippedWeapon = w_inventory[0];
        GameObject temp = Instantiate(equippedWeapon, transform.position, Quaternion.identity) as GameObject;
        temp.transform.parent = transform;
    }

    public void AddWeapon(string weaponToAdd)
    {
        w_inventory.Add( allWeapons.FindWeapon(weaponToAdd));
    }

    // true = switch forward, false = switch back
    public void SwitchWeapon(bool next)
    {
        if (next)
        {
            weaponIndex++;
            if (weaponIndex >= w_inventory.Count)
                weaponIndex = 0;
        }
        else
        {
            weaponIndex--;
            if (weaponIndex < 0)
                weaponIndex = w_inventory.Count - 1;
        }

        equippedWeapon = w_inventory[weaponIndex];

        // MAYBE THERES A BETTER WAY TO DO THIS
        // destroy current weapon
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        // instantiate new weapon
        GameObject temp = Instantiate(equippedWeapon, transform.position, Quaternion.identity) as GameObject;
        temp.transform.parent = transform;
        temp.transform.rotation = transform.rotation;
    }

}
