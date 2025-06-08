using CKT;
using System.Collections;
using UnityEngine;

namespace CKT
{
    public class ImageParts_HitScatter : ImageParts, ISkillable
    {
        private void Awake()
        {
            base.Init("FieldParts/FieldParts_HitScatter");
        }

        float _scatterAngle = 9f;

        public SkillType SkillType => SkillType.Hit;

        public string SkillName => "HitScatter";

        public IEnumerator SkillCoroutine(GameObject origin, int level, SkillManager skillManager)
        {
            Debug.Log($"{SkillName}, Level+{level}");

            int scatterCount = level + 1;
            Vector3 originUp = origin.transform.up;

            for (int k = 0; k < scatterCount; k++)
            {
                //분산 각도
                float sign = 0;
                if (scatterCount % 2 == 0)
                {
                    sign = ((k % 2 == 0) ? -1 : 1) * (Mathf.Floor(k / 2.0f) + 0.5f);
                }
                else
                {
                    sign = ((k % 2 == 0) ? 1 : -1) * Mathf.Ceil(k / 2.0f);
                }
                Vector2 scatterDir = Util.RotateVector(originUp, (sign * _scatterAngle)).normalized;

                GameObject hitScatterCopy = YSJ.Managers.Pool.InstPrefab("HitScatter");
                hitScatterCopy.transform.position = origin.transform.position;
                hitScatterCopy.transform.up = scatterDir;
                hitScatterCopy.name = "HitScatter";
                //hitScatterCopy.GetComponent<Projectile>().SkillManager = skillManager;
            }

            yield return null;
        }
    }
}