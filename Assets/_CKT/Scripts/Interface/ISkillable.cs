using System.Collections;
using UnityEngine;

public enum SkillType { Cast, Hit, Passive }

public interface ISkillable
{
    public SkillType SkillType { get; }

    public string SkillName { get; }

    public IEnumerator SkillCoroutine(GameObject origin, int level);
}
