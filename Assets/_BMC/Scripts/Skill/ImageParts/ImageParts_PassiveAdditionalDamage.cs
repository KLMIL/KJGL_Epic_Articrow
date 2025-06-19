using CKT;
using System.Collections;
using UnityEngine;

namespace BMC
{
    public class ImageParts_PassiveAdditionalDamage : ImageParts, ISkillable
    {
        public override Define.SkillType SkillType => Define.SkillType.Passive;

        public override string SkillName => "PassiveAdditionalDamage";

        public IEnumerator SkillCoroutine(Vector3 position, Vector3 directoin, int level, SkillManager skillManager)
        {
            throw new System.NotImplementedException();
        }
    }
}