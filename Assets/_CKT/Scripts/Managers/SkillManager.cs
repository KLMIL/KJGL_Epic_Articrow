using System;
using System.Collections;
using UnityEngine;

namespace CKT
{
    [System.Serializable]
    public class SkillManager
    {
        float _scatterAngle = 3f;

        int _castScatterLevel = 0;
        int _castAdditionalLevel = 0;
        int _castExplosionLevel = 0;

        int _hitScatterLevel = 0;

        #region [Inventory에서 호출할 함수]
        public void InitLevel()
        {
            _scatterAngle = 3f;

            _castScatterLevel = 0;
            _castAdditionalLevel = 0;
            _castExplosionLevel = 0;

            _hitScatterLevel = 0;
        }
        #endregion

        #region [ICastEffectable, IHitEffectable에서 호출할 함수]
        public void CastScatterLevelUp(int amount)
        {
            _castScatterLevel += amount;
        }

        public void CastAdditionalLevelUp(int amount)
        {
            _castAdditionalLevel += amount;
        }

        public void CastExplosionLevelUp(int amount)
        {
            _castExplosionLevel += amount;
        }

        

        public void HitScatterLevelUp(int amount)
        {
            _hitScatterLevel += amount;
        }
        #endregion

        #region [EquipedArtifact에서 호출할 함수]
        public void CastScatter(GameObject origin)
        {
            int scatterCount = _castScatterLevel * 2;

            for (int k = 0; k < scatterCount; k++)
            {
                //분산 각도
                float sign = ((k % 2 == 0) ? 1 : -1) * (Mathf.Floor(k / 2.0f) + 1);
                Vector2 scatterDir = RotateVector(origin.transform.up, (sign * _scatterAngle)).normalized;

                GameObject castScatterCopy = YSJ.Managers.Pool.InstPrefab(origin.name, null, origin.transform.position);
                castScatterCopy.transform.up = scatterDir;
                castScatterCopy.name = origin.name;
            }
        }

        public IEnumerator CastAdditionalCoroutine(GameObject origin)
        {
            Vector3 startPos = origin.transform.position;
            
            for (int i = 0; i < _castAdditionalLevel; i++)
            {
                yield return new WaitForSeconds(0.05f);
                GameObject castAdditionalCopy = YSJ.Managers.Pool.InstPrefab(origin.name, null, startPos);
                castAdditionalCopy.transform.up = origin.transform.up;
                castAdditionalCopy.name = origin.name;
                CastScatter(castAdditionalCopy);
            }
        }

        public IEnumerator CastExplosionCoroutine(GameObject origin)
        {
            Vector3 startPos = origin.transform.position;

            for (int i = 0; i < _castExplosionLevel; i++)
            {
                GameObject castExplosion = YSJ.Managers.Pool.InstPrefab("Explosion", null, startPos);
                yield return new WaitForSeconds(0.05f);
            }
        }
        #endregion

        #region [Utils]
        //TODO : Utils에 해당 메소드 넣기
        protected Vector2 RotateVector(Vector2 vector, float angleDegrees)
        {
            float angleRad = angleDegrees * Mathf.Deg2Rad; // 도를 라디안으로 변환
            float cos = Mathf.Cos(angleRad);
            float sin = Mathf.Sin(angleRad);

            // 2D 벡터 회전 공식 적용
            float newX = vector.x * cos - vector.y * sin;
            float newY = vector.x * sin + vector.y * cos;

            return new Vector2(newX, newY);
        }
        #endregion
    }
}