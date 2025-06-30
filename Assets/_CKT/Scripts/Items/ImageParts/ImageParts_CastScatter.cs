using BMC;
using System.Collections;
using UnityEngine;

namespace CKT
{
    public class ImageParts_CastScatter : ImageParts, ISkillable
    {
        float _scatterAngle = 12f;

        public override Define.SkillType SkillType => Define.SkillType.Cast;

        public override string SkillName => "CastScatter";

        public IEnumerator SkillCoroutine(Vector3 position, Vector3 direction, int level, SkillManager skillManager)
        {
            Debug.Log($"{SkillName}, Level+{level}");

            //int scatterCount = level + 1;

            //for (int k = 0; k < scatterCount; k++)
            for (int k = 0; k < (level * 2); k++)
            {
                //분산 각도
                float sign = 0;
                /*if (scatterCount % 2 == 0)
                {
                    sign = ((k % 2 == 0) ? -1 : 1) * (Mathf.Floor(k / 2.0f) + 0.5f);
                }
                else*/
                {
                    sign = ((k % 2 == 0) ? 1 : -1) * Mathf.Ceil((k+1) / 2.0f);
                }
                Vector2 scatterDir = Util.RotateVector(direction, (sign * _scatterAngle)).normalized;

                ArtifactSO artifactSO = BMC.PlayerManager.Instance.Inventory.SkillManager.GetArtifactSOFuncT0.Trigger();
                GameObject castScatterCopy = YSJ.Managers.TestPool.Get<GameObject>(artifactSO.ProjectilePoolID);
                castScatterCopy.transform.position = position;
                castScatterCopy.transform.up = scatterDir;
                castScatterCopy.GetComponent<Projectile>().Init(true);
            }

            //origin.SetActive(false);
            yield return null;
        }
    }
}