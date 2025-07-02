using BMC;
using System.Collections;
using UnityEngine;

namespace CKT
{
    public class ImageParts_CastExplosion : ImageParts, ISkillable
    {
        public override Define.SkillType SkillType => Define.SkillType.Cast;

        public override string SkillName => "CastExplosion";

        public IEnumerator SkillCoroutine(Vector3 position, Vector3 direction, int level, SkillManager skillManager)
        {
            Debug.Log($"{SkillName}, Level+{level}");

            Vector3 startPos = position;

            /*for (int i = 0; i < level; i++)
            {
                GameObject castExplosion = YSJ.Managers.Pool.InstPrefab("CastExplosion");
                castExplosion.transform.position = startPos;
                yield return new WaitForSeconds(0.05f);
            }*/

            YSJ.Managers.Sound.PlaySFX(Define.SFX.CastExplosion);
            GameObject castExplosion = YSJ.Managers.Pool.Get<GameObject>(Define.PoolID.CastExplosion);
            castExplosion.transform.position = startPos;
            castExplosion.GetComponent<Explosion>().Init(level);

            yield return null;

        }
    }
}