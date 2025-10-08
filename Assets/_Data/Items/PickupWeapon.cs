using System;
using UnityEngine;

public class PickupWeapon : Interactable
{
    [SerializeField] protected WeaponDataSO weaponData;
    [SerializeField] protected Weapon weapon;

    [SerializeField] protected BackupWeaponModel[] models;

    protected bool oldWeapon;

    protected override void Start()
    {
        base.Start();
        if (!oldWeapon)
            weapon = new Weapon(weaponData);
        SetupGameObject();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadBackupWeaponModels();
    }

    protected void LoadBackupWeaponModels()
    {
        if (models is { Length: > 0 }) return;
        models = GetComponentsInChildren<BackupWeaponModel>(true);
        Debug.Log(transform.name + " LoadBackupWeaponModels", gameObject);
    }

    public void SetupPickupWeapon(Weapon wp, Transform trans)
    {
        oldWeapon = true;
        weapon = wp;
        weaponData = wp.weaponData;
        transform.position = trans.position + new Vector3(0, 0.5f, 0);
        SetupGameObject();
    }

    [ContextMenu("Update Item Model")]
    public void SetupGameObject()
    {
        gameObject.name = "Pickup" + weaponData.weaponType.ToString();
        SetupWeaponModel();
    }

    public void SetupWeaponModel()
    {
        foreach (BackupWeaponModel model in models)
        {
            model.gameObject.SetActive(false);

            if (model.weaponType != weaponData.weaponType) continue;
            model.gameObject.SetActive(true);
            UpdateMeshAndMaterial(model.GetComponentInChildren<MeshRenderer>());
        }
    }

    public override void Interaction()
    {
        inventory.PickupWeapon(weapon);
        transform.name = "PickupWeapon";
        WeaponSpawner.Instance.Despawn(gameObject);
    }
}