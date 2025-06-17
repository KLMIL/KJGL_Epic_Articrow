using BMC;
using System.Collections;
using UnityEngine;

namespace CKT
{
    public class ImageParts_CastScatter : ImageParts, ISkillable
    {
        float _scatterAngle = 9f;

        private void Awake()
        {
            base.Init("FieldParts/FieldParts_CastScatter", 0f);
        }

        public SkillType SkillType => SkillType.Cast;

        public string SkillName => "CastScatter";

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

                Define.PoolID poolID = skillManager.GetProjectilePoolID.Trigger();
                GameObject castScatterCopy = YSJ.Managers.TestPool.Get<GameObject>(poolID);
                castScatterCopy.transform.position = origin.transform.position;
                castScatterCopy.transform.up = scatterDir;
                castScatterCopy.GetComponent<Projectile>().SkillManager = skillManager;
            }

            origin.SetActive(false);
            PlayerManager.Instance.PlayerStatus.SpendMana(base._manaCost * level);
            yield return null;
        }
    }
}