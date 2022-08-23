using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Attack",menuName ="Attack/AttackData")]
public class AttackData_SO : ScriptableObject
{
    public float attackRange;

    public float skillRange;

    public float coolDown;

    public int minDamage;

    public int maxDamage;

    public float criticalMultiplier;

    public float criticalChance;
}
