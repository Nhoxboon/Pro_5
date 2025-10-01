using System;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] protected Weapon weapon;

    protected void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponentInChildren(out PlayerAttack atk))
            atk.PickupWeapon(weapon);
    }
}