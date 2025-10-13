using System;
using UnityEngine;

[Serializable]
public struct AttackData
{
    public float attackRange;
    public float moveSpeed;
    public float attackIndex;
    [Range(1, 2)]
    public float animationSpeed;
}