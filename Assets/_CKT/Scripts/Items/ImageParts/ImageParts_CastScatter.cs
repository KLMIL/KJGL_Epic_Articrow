using BMC;
using System.Collections;
using UnityEngine;

namespace CKT
{
    public class ImageParts_CastScatter : ImageParts, ISkillable
    {
        float _manaCost = 5f;
        float _scatterAngle = 15f;

        private void Awake()
        {
            base.Init("FieldParts/FieldParts_CastScatter");
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

                GameObject castScatterCopy = YSJ.Managers.Pool.InstPrefab(origin.name);
                castScatterCopy.transform.SetParent(origin.transform.parent);
                castScatterCopy.transform.position = origin.transform.position;
                castScatterCopy.transform.up = scatterDir;
                castScatterCopy.name = origin.name;
                castScatterCopy.GetComponent<Projectile>().SkillManager = skillManager;
            }

            origin.SetActive(false);
            PlayerManager.Instance.PlayerStatus.SpendMana(_manaCost);
            yield return null;
        }
    }
}