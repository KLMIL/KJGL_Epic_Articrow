using BMC;
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
            _player = _player ?? FindAnyObjectByType<BMC.PlayerManager>().transform;
            Vector3 originUp = (position - _player.position).normalized;

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
                GameObject hitScatterCopy = YSJ.Managers.TestPool.Get<GameObject>(poolID);
                hitScatterCopy.transform.position = position;
                hitScatterCopy.transform.up = scatterDir;

                Projectile[] projectiles = hitScatterCopy.GetComponentsInChildren<Projectile>();
                for (int i = 0; i < projectiles.Length; i++)
                {
                    projectiles[i].CurPenetration += 1;
                }
            }

            yield return null;
        }
    }
}