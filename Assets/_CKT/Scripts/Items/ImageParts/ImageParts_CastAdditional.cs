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

        public IEnumerator SkillCoroutine(GameObject origin, int level, SkillManager skillManager)
        {
            Debug.Log($"{SkillName}, Level+{level}");
            
            Vector3 startPos = origin.transform.position;

            for (int i = 0; i < level; i++)
            {
                yield return new WaitForSeconds(0.05f);
                GameObject castAdditionalCopy = YSJ.Managers.Pool.InstPrefab(origin.name);
                castAdditionalCopy.transform.SetParent(origin.transform.parent);
                castAdditionalCopy.transform.position = startPos;
                castAdditionalCopy.transform.up = origin.transform.up;
                castAdditionalCopy.name = origin.name;
                castAdditionalCopy.GetComponent<Projectile>().SkillManager = skillManager;

                if (skillManager.CastSkillDict.ContainsKey("CastScatter"))
                {
                    StartCoroutine(skillManager.CastSkillDict["CastScatter"](castAdditionalCopy));
                }
            }
        }
    }
}