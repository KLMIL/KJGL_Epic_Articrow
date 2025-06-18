using CKT;
using System.Collections;
using UnityEngine;



public interface ISkillable
{
    public IEnumerator SkillCoroutine(Vector3 position, Vector3 directoin, int level, SkillManager skillManager);
}
