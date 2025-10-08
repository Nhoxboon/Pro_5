using System;
using UnityEngine;

[Serializable]
public struct AmmoData
{
    public WeaponType weaponType;
    [Range(10,100)] 
    public int minAmount;
    [Range(10,100)] 
    public int maxAmount;
}