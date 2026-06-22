
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NhoxSingleton<GameManager>
{
    public bool friendlyFire;

    public void SetDefaultWeapons()
    {
        List<WeaponDataSO> newList = UIManager.Instance.UIWeaponSel.SelectedWeaponsData();
        if (newList.Count == 0) return;
        PlayerCtrl.Instance.PlayerInventory.SetDefaultWeapons(newList);
    }
}
