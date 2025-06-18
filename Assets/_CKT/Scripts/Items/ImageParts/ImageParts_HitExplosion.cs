using BMC;
using System.Collections;
using UnityEngine;

namespace CKT
{
    public class ImageParts_HitExplosion : ImageParts, ISkillable
    {
        public override Define.SkillType SkillType => Define.SkillType.Hit;

        public override string SkillName => "HitExplosion";

        public IEnumerator SkillCoroutine(Vector3 position, Vector3 direction, int level, SkillManager skillManager)
        {
            Debug.Log($"{SkillName}, Level+{level}");

            Vector3 startPos = position;

            /*for (int i = 0; i < level; i++)
            {
                YSJ.Managers.Sound.PlaySFX(Define.SFX.HitExplosion);
                GameObject hitExplosion = YSJ.Managers.Pool.InstPrefab("HitExplosion");
                hitExplosion.transform.position = startPos;
                yield return new WaitForSeconds(0.05f);
            }*/

            YSJ.Managers.Sound.PlaySFX(Define.SFX.HitExplosion);
            GameObject hitExplosion = YSJ.Managers.TestPool.Get<GameObject>(Define.PoolID.HitExplosion);
            hitExplosion.transform.position = startPos;
            hitExplosion.GetComponent<Explosion>().Init(level);

            yield return null;
        }
    }
}