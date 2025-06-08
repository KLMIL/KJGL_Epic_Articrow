using System.Collections;
using UnityEngine;

namespace CKT
{
    public class ImageParts_HitExplosion : ImageParts, ISkillable
    {
        private void Awake()
        {
            base.Init("FieldParts/FieldParts_HitExplosion");
        }

        public SkillType SkillType => SkillType.Hit;

        public string SkillName => "HitExplosion";

        public IEnumerator SkillCoroutine(GameObject origin, int level, SkillManager skillManager)
        {
            Debug.Log($"{SkillName}, Level+{level}");

            Vector3 startPos = origin.transform.position;

            for (int i = 0; i < level; i++)
            {
                //TODO : 사운드_HitExplosion
                
                GameObject castExplosion = YSJ.Managers.Pool.InstPrefab("HitExplosion");
                castExplosion.transform.position = startPos;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}