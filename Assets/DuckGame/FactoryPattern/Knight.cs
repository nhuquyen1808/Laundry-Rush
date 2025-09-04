using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
   [SerializeField] WeaponFactory weaponFactory;

    IWeapon weapon;

    private void Start()
    {
        weapon = weaponFactory.CreateWeapon();




        /*switch (weaponType)
        {
            case "Sword":
                weapon = new Sword();
                break;
            case "Bow":
                break;
        }*/
    }
    public void Attack()=> weapon?.Attack();
}
