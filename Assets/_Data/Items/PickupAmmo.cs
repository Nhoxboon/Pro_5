using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickupAmmo : Interactable
{
    [SerializeField] protected AmmoBoxType boxType;
    
    [SerializeField] protected List<AmmoData> smallBox;
    [SerializeField] protected List<AmmoData> bigBox;
    
    [SerializeField] protected GameObject[] boxModels;

    protected override void Start()
    {
        base.Start();
        SetupBoxModel();
    }

    protected void SetupBoxModel()
    {
        for (int i = 0; i < boxModels.Length; i++)
        {
            boxModels[i].SetActive(false);
            if (i != (int)boxType) continue;
            boxModels[i].SetActive(true);
            UpdateMeshAndMaterial(boxModels[i].GetComponentInChildren<MeshRenderer>());
        }
    }

    public override void Interaction()
    {
        List<AmmoData> currentAmmoList = smallBox;
        if(boxType == AmmoBoxType.BigBox)
            currentAmmoList = bigBox;
        foreach (AmmoData ammo in currentAmmoList)
        {
            Weapon weapon = inventory.WeaponInSlots(ammo.weaponType);
            AddBulletToWeapon(weapon, GetRandomAmmoAmount(ammo));
        }
        AmmoSpawner.Instance.Despawn(gameObject);
    }

    protected void AddBulletToWeapon(Weapon weapon, int amount)
    {
        if (weapon == null) return;
        weapon.totalReserveAmmo += amount;
    }

    protected int GetRandomAmmoAmount(AmmoData ammoData)
    {
        float min = Mathf.Min(ammoData.minAmount, ammoData.maxAmount);
        float max = Mathf.Max(ammoData.minAmount, ammoData.maxAmount);
        
        return Mathf.RoundToInt(Random.Range(min, max));
    }
}