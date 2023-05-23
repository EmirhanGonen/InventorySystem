using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObjects/WeaponData")]
public class WeaponData : InventoryItemBase
{
    [SerializeField] private int _damage = 0;

    [SerializeField, Range(0.00f, 10.00f)] private float _attackRange = 0.00f;

    [SerializeField] private LayerMask _hitMask = 0;


    public int Damage => _damage;
    public float AttackRange => _attackRange;
    public LayerMask HitMask => _hitMask;


    /*public override void Use()
    {
        base.Use();
        Debug.Log("s");
    }*/
}