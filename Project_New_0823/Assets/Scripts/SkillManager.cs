using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> skillPrefabs;

    public void CastSkill(int index)
    {
        GameObject skillPrefab = skillPrefabs[index];
        Instantiate(skillPrefab, transform.position, Quaternion.identity);        
    }
}
