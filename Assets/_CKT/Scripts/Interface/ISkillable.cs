using CKT;
using System.Collections;
using UnityEngine;



public interface ISkillable
{
    /// <summary>
    /// Dictionary에 등록할 실제 스킬 효과
    /// </summary>
    /// <param name="position"></param>
    /// <param name="directoin"></param>
    /// <param name="level"></param>
    /// <param name="skillManager"></param>
    /// <returns></returns>
    public IEnumerator SkillCoroutine(Vector3 position, Vector3 directoin, int level, SkillManager skillManager);
}
