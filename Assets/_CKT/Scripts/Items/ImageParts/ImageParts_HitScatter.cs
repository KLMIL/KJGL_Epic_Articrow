using System.Collections;
using UnityEngine;

namespace CKT
{
    public class ImageParts_HitScatter : ImageParts, ISkillable
    {
        Transform _player;
        float _scatterAngle = 9f;

        public override Define.SkillType SkillType => Define.SkillType.Hit;

        public override string SkillName => "HitScatter";

        public IEnumerator SkillCoroutine(Vector3 position, Vector3 direction, int level, SkillManager skillManager)
        {
            Debug.Log($"{SkillName}, Level+{level}");

            int scatterCount = level + 1;
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
                Vector2 scatterDir = Util.RotateVector(direction, (sign * _scatterAngle)).normalized;

                //ArtifactSO artifactSO = BMC.PlayerManager.Instance.Inventory.SkillManager.GetArtifactSOFuncT0.Trigger();
                //GameObject hitScatterCopy = YSJ.Managers.TestPool.Get<GameObject>(artifactSO.ProjectilePoolID);
                //hitScatterCopy.transform.position = position;
                //hitScatterCopy.transform.up = scatterDir;

                //Projectile[] projectiles = hitScatterCopy.GetComponentsInChildren<Projectile>();
                //for (int i = 0; i < projectiles.Length; i++)
                //{
                //    projectiles[i].Init(false);
                //}
            }

            yield return null;
        }
    }
}