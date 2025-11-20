using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EMeleeWeaponDataSO", menuName = "Enemy Data/Melee Weapon Data")]
public class EMeleeWeaponDataSO : ScriptableObject
{
    public List<AttackData_EnemyMelee> attackData;
    public float turnSpeed = 5f;
}