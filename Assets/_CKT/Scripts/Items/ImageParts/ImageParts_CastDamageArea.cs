using System.Collections;
using UnityEngine;

namespace CKT
{
    public class ImageParts_CastDamageArea : ImageParts, ISkillable
    {
        private void Awake()
        {
            base.Init("FieldParts/FieldParts_CastDamageArea");
        }

        public SkillType SkillType => SkillType.Cast;

        public string SkillName => "CastDamageArea";

        public IEnumerator SkillCoroutine(GameObject origin, int level)
        {
            Debug.Log($"{SkillName}, Level+{level}");

            Vector3 startPos = origin.transform.position;

            for (int i = 0; i < level; i++)
            {
                //TODO : 사운드_CastDamageArea
                
                GameObject castDamageArea = YSJ.Managers.Pool.InstPrefab("CastDamageArea");
                castDamageArea.transform.position = startPos;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}