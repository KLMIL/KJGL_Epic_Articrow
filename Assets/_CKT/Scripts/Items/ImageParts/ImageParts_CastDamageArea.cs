using BMC;
using System.Collections;
using UnityEngine;
using YSJ;

namespace CKT
{
    public class ImageParts_CastDamageArea : ImageParts, ISkillable
    {
        public override Define.SkillType SkillType => Define.SkillType.Cast;

        public override string SkillName => "CastDamageArea";

        public IEnumerator SkillCoroutine(Vector3 position, Vector3 direction, int level, SkillManager skillManager)
        {
            Debug.Log($"{SkillName}, Level+{level}");

            Vector3 startPos = position;

            /*for (int i = 0; i < level; i++)
            {
                GameObject castDamageArea = YSJ.Managers.Pool.InstPrefab("CastDamageArea");
                castDamageArea.transform.position = startPos;
                yield return new WaitForSeconds(0.05f);
            }*/

            //Managers.Sound.PlaySFX(Define.SFX.CastDamageArea);
            //GameObject castDamageArea = YSJ.Managers.TestPool.Get<GameObject>(Define.PoolID.CastDamageArea);
            //castDamageArea.transform.position = startPos;
            //castDamageArea.GetComponent<DamageArea>().Init(level);

            yield return null;
        }
    }
}