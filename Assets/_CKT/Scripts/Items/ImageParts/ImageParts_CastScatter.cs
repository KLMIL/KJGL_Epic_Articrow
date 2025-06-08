using System.Collections;
using UnityEngine;

namespace CKT
{
    public class ImageParts_CastScatter : ImageParts, ISkillable
    {
        float _scatterAngle = 9f;

        private void Awake()
        {
            base.Init("FieldParts/FieldParts_CastScatter");
        }

        public SkillType SkillType => SkillType.Cast;

        public string SkillName => "CastScatter";

        public IEnumerator SkillCoroutine(GameObject origin, int level)
        {
            Debug.Log($"{SkillName}, Level+{level}");

            Scatter(origin, origin.name, level, true);
            yield return null;
        }

        void Scatter(GameObject origin, string prefabName, int level, bool includeOrigin)
        {
            Vector3 originUp = origin.transform.up;

            //level이 0일 때는 for문 스킵 (레벨 + origin)
            int scatterCount = (level == 0) ? 0 : ((includeOrigin) ? (level + 1) : level);
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
                Vector2 scatterDir = RotateVector(originUp, (sign * _scatterAngle)).normalized;

                //본체 포함일 때 = 0번째는 본체 + 회전만
                if ((k == 0) && includeOrigin)
                {
                    origin.transform.up = scatterDir;
                }
                else
                {
                    GameObject castScatterCopy = YSJ.Managers.Pool.InstPrefab(prefabName);
                    castScatterCopy.transform.position = origin.transform.position;
                    castScatterCopy.transform.up = scatterDir;
                    castScatterCopy.name = prefabName;
                    //castScatterCopy.GetComponent<Projectile>().SkillManager = this;
                    //TODO : CastScatter 완성하기
                }
            }
        }

        Vector2 RotateVector(Vector2 vector, float angleDegrees)
        {
            float angleRad = angleDegrees * Mathf.Deg2Rad; // 도를 라디안으로 변환
            float cos = Mathf.Cos(angleRad);
            float sin = Mathf.Sin(angleRad);

            // 2D 벡터 회전 공식 적용
            float newX = vector.x * cos - vector.y * sin;
            float newY = vector.x * sin + vector.y * cos;

            return new Vector2(newX, newY);
        }
    }
}