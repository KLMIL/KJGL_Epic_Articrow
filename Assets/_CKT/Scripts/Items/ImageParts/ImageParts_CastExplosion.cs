using CKT;
using System.Collections;
using UnityEngine;

namespace CKT
{
    public class ImageParts_CastExplosion : ImageParts, ISkillable
    {
        private void Awake()
        {
            base.Init("FieldParts/FieldParts_CastExplosion");
        }

        public SkillType SkillType => SkillType.Cast;

        public string SkillName => "CastExplosion";

        public IEnumerator SkillCoroutine(GameObject origin, int level, SkillManager skillManager)
        {
            Debug.Log($"{SkillName}, Level+{level}");

            Vector3 startPos = origin.transform.position;

            for (int i = 0; i < level; i++)
            {
                //TODO : 사운드_CastExplosion
                YSJ.Managers.Sound.PlaySFX(Define.SFX.CastExplosion);

                GameObject castExplosion = YSJ.Managers.Pool.InstPrefab("CastExplosion");
                castExplosion.transform.position = startPos;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}