using System;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] protected WeaponDataSO weaponData;

    protected void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponentInChildren(out PlayerInventory inventory))
            inventory.PickupWeapon(weaponData);
    }
}