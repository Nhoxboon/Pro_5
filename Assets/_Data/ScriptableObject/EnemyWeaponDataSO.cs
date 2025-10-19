
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWeaponDataSO", menuName = "Enemy Data/Melee Weapon Data")]
public class EnemyWeaponDataSO : ScriptableObject
{
    public List<AttackData_EnemyMelee> attackData;
    public float turnSpeed = 5f;
}
