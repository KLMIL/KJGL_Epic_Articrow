using CKT;
using System.Collections;
using UnityEngine;

namespace BMC
{
    public class ImageParts_CastDashToDirection : ImageParts, ISkillable
    {
        public override Define.SkillType SkillType => Define.SkillType.Cast;

        public override string SkillName => "CastDashToDirection";

        public IEnumerator SkillCoroutine(Vector3 position, Vector3 directoin, int level, SkillManager skillManager)
        {
            Debug.Log($"{SkillName}, Level+{level}");
            PlayerManager.Instance.PlayerDash.DashSkill(directoin);
            Debug.Log("대시 스킬");
            yield return null;
        }
    }
}