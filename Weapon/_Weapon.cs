using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : Item  /*IGrapable*/
{
    //[SerializeField] private WeaponData _data = null;

    //public WeaponData Data { set => _data = value; }

    /*public override void Use()
    {
        Debug.Log("SendedRay");
        RaycastHit2D _hit = Physics2D.Raycast(transform.position, transform.forward, (ItemData as WeaponData).AttackDistance);
    }*/
}