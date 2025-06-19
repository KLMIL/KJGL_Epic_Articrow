using BMC;
using System.Collections;
using UnityEngine;

namespace CKT
{
    public class ImageParts_CastAdditional : ImageParts, ISkillable
    {
        public override Define.SkillType SkillType => Define.SkillType.Cast;

        public override string SkillName => "CastAdditional";

        public IEnumerator SkillCoroutine(Vector3 position, Vector3 direction, int level, SkillManager skillManager)
        {
            Debug.Log($"{SkillName}, Level+{level}");
            
            for (int i = 0; i < level; i++)
            {
                yield return new WaitForSeconds(0.05f);

                Define.PoolID poolID = skillManager.GetProjectilePoolID.Trigger();
                GameObject castAdditionalCopy = YSJ.Managers.TestPool.Get<GameObject>(poolID);

                castAdditionalCopy.transform.position = position;
                castAdditionalCopy.transform.up = direction;
                Projectile projectile = castAdditionalCopy.GetComponent<Projectile>();
                projectile.SkillManager = skillManager;

                if (skillManager.CastSkillDict.ContainsKey("CastScatter"))
                {
                    StartCoroutine(skillManager.CastSkillDict["CastScatter"](position, direction));
                }
            }
        }
    }
}