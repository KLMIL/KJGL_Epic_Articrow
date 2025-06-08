using CKT;
using System.Collections;
using UnityEngine;

namespace CKT
{
    public class ImageParts_CastAdditional : ImageParts, ISkillable
    {
        private void Awake()
        {
            base.Init("FieldParts/FieldParts_CastAdditional");
        }

        public SkillType SkillType => SkillType.Cast;

        public string SkillName => "CastAdditional";

        public IEnumerator SkillCoroutine(GameObject origin, int level)
        {
            Debug.Log($"{SkillName}, Level+{level}");
            
            Vector3 startPos = origin.transform.position;

            for (int i = 0; i < level; i++)
            {
                yield return new WaitForSeconds(0.05f);
                GameObject castAdditionalCopy = YSJ.Managers.Pool.InstPrefab(origin.name);
                castAdditionalCopy.transform.position = startPos;
                castAdditionalCopy.transform.up = origin.transform.up;
                castAdditionalCopy.name = origin.name;
                //castAdditionalCopy.GetComponent<Projectile>().SkillManager = this;

                //Scatter(castAdditionalCopy, origin.name, _castScatterLevel, true);
                //TODO : CastAdditional Scatter 완성하기
            }
        }
    }
}