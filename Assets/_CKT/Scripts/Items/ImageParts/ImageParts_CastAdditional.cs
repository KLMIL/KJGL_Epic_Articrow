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

                //ArtifactSO artifactSO = BMC.PlayerManager.Instance.Inventory.SkillManager.GetArtifactSOFuncT0.Trigger();
                //GameObject castAdditionalCopy = YSJ.Managers.TestPool.Get<GameObject>(artifactSO.ProjectilePoolID);

                //castAdditionalCopy.transform.position = position;
                //castAdditionalCopy.transform.up = direction;
                //Projectile[] projectile = castAdditionalCopy.GetComponentsInChildren<Projectile>();
                //for (int k = 0; k < projectile.Length; k++)
                //{
                //    projectile[k].Init(true);
                //}

                if (skillManager.CastSkillDict.ContainsKey("CastScatter"))
                {
                    StartCoroutine(skillManager.CastSkillDict["CastScatter"](position, direction));
                }
            }
        }
    }
}