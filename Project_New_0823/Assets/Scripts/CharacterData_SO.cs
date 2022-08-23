using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Data", menuName ="Character/Data")]
public class CharacterData_SO : ScriptableObject
{
    public int maxHealth;

    public int currentHealth;

    public int baseDefence;

    public int currentDefence;
}
