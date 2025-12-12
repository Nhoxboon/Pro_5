using System;
using UnityEngine;

[Serializable]
public struct AttackData_EnemyMelee
{
    public string attackName;
    public int attackDamage;
    public float attackRange;
    public float moveSpeed;
    public float attackIndex;
    [Range(1, 2)] public float animationSpeed;
    public AttackTypeMelee attackType;
}