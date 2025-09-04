using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public interface IWeapon
{
    public void Attack();

}




public class Sword : IWeapon
{
    public void Attack()
    {
        Debug.Log("Swinging the sword !!");
    }
}

public class Bow : IWeapon
{
    public void Attack()
    {
        Debug.Log("Shooting an Arrow");
    }
}



public abstract class WeaponFactory : ScriptableObject
{
    public abstract IWeapon CreateWeapon();

}

[CreateAssetMenu(fileName = "SwordFactory", menuName ="Weapon Factory/Sword")]

public class SwordFactory : WeaponFactory
{
    public override IWeapon CreateWeapon()
    {
        return new Sword();
    }
}


[CreateAssetMenu(fileName = "SwordFactory", menuName = "Weapon Factory/Bow")]

public class BowFactory : WeaponFactory
{
    public override IWeapon CreateWeapon()
    {
        return new Bow();
    }
}
