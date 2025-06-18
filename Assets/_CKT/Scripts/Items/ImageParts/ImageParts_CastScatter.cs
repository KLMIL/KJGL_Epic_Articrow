using BMC;
using System.Collections;
using UnityEngine;

namespace CKT
{
    public class ImageParts_CastScatter : ImageParts, ISkillable
    {
        float _scatterAngle = 9f;

        public override Define.SkillType SkillType => Define.SkillType.Cast;

        public override string SkillName => "CastScatter";

        public IEnumerator SkillCoroutine(Vector3 position, Vector3 direction, int level, SkillManager skillManager)
        {
            Debug.Log($"{SkillName}, Level+{level}");

            int scatterCount = level + 1;
            Vector3 originUp = direction;

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

                Define.PoolID poolID = skillManager.GetProjectilePoolID.Trigger();
                GameObject castScatterCopy = YSJ.Managers.TestPool.Get<GameObject>(poolID);
                castScatterCopy.transform.position = position;
                castScatterCopy.transform.up = scatterDir;
                castScatterCopy.GetComponent<Projectile>().SkillManager = skillManager;
            }

            //origin.SetActive(false);
            yield return null;
        }
    }
}