using BMC;
using System.Collections;
using UnityEngine;

namespace CKT
{
    public class ImageParts_HitDamageArea : ImageParts, ISkillable
    {
        public override Define.SkillType SkillType => Define.SkillType.Hit;

        public override string SkillName => "HitDamageArea";

        public IEnumerator SkillCoroutine(Vector3 position, Vector3 direction, int level, SkillManager skillManager)
        {
            Debug.Log($"{SkillName}, Level+{level}");

            Vector3 startPos = position;

            /*for (int i = 0; i < level; i++)
            {

                GameObject hitDamageArea = YSJ.Managers.Pool.InstPrefab("HitDamageArea");
                hitDamageArea.transform.position = startPos;
                yield return new WaitForSeconds(0.05f);
            }*/

            YSJ.Managers.Sound.PlaySFX(Define.SFX.HitDamageArea);
            //GameObject hitDamageArea = YSJ.Managers.TestPool.Get<GameObject>(Define.PoolID.HitDamageArea);
            //hitDamageArea.transform.position = startPos;
            //hitDamageArea.GetComponent<DamageArea>().Init(level);

            yield return null;
        }
    }
}