using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponHolder : MonoBehaviour
{
    private WeaponData _lastData = null;

    private void OnDrawGizmos()
    {
        if (_lastData)
        {
            Vector2 _direction = transform.parent.localScale.x.Equals(-1) ? -transform.right : transform.right;
            Gizmos.DrawRay(transform.position, _direction * _lastData.AttackRange);
        }
    }


    public void SendRay<T>(T _weaponData) where T : WeaponData
    {
        _lastData = _weaponData;

        Vector2 _direction = transform.parent.localScale.x.Equals(-1) ? -transform.right : transform.right;

        RaycastHit2D _hit = Physics2D.Raycast(transform.position, _direction, _weaponData.AttackRange, _weaponData.HitMask);

        Debug.Log("Ray Sended");

        if (_hit.collider)
            if (_hit.collider.TryGetComponent(out IDamagable damagable))
                damagable.TakeDamage(_weaponData.Damage);
    }
}