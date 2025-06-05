using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CKT
{
    [System.Serializable]
    public class SkillManager
    {
        float _scatterAngle;

        int _castScatterLevel;
        int _castAdditionalLevel;
        int _castExplosionLevel;
        int _castDamageAreaLevel;

        int _hitScatterLevel;
        int _hitExplosionLevel;
        int _hitDamageAreaLevel;

        #region [Passive, Cast, Hit]
        public List<Func<GameObject, IEnumerator>> CastSkillList => _castSkillList;
        List<Func<GameObject, IEnumerator>> _castSkillList = new List<Func<GameObject, IEnumerator>>();

        public List<Func<GameObject, IEnumerator>> HitSkillList => _hitSkillList;
        List<Func<GameObject, IEnumerator>> _hitSkillList = new List<Func<GameObject, IEnumerator>>();
        #endregion

        #region [Inventory에서 호출할 함수]
        public void InitLevel()
        {
            _scatterAngle = 3f;

            _castScatterLevel = 0;
            _castAdditionalLevel = 0;
            _castExplosionLevel = 0;
            _castDamageAreaLevel = 0;

            _hitScatterLevel = 0;
            _hitExplosionLevel = 0;
            _hitDamageAreaLevel = 0;

            //CastSkill
            _castSkillList.Clear();
            _castSkillList.Add((obj) => CastScatterCoroutine(obj));
            _castSkillList.Add((obj) => CastAdditionalCoroutine(obj));
            _castSkillList.Add((obj) => CastExplosionCoroutine(obj));
            _castSkillList.Add((obj) => CastDamageAreaCoroutine(obj));
            
            //HitSkill
            _hitSkillList.Clear();
            _hitSkillList.Add((obj) => HitScatter(obj));
            _hitSkillList.Add((obj) => HitExplosionCoroutine(obj));
            _hitSkillList.Add((obj) => HitDamageAreaCoroutine(obj));
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

        public void CastDamageAreaLevelUp(int amount)
        {
            _castDamageAreaLevel += amount;
        }



        public void HitScatterLevelUp(int amount)
        {
            _hitScatterLevel += amount;
        }

        public void HitExplosionLevelUp(int amount)
        {
            _hitExplosionLevel += amount;
        }

        public void HitDamageAreaLevelUp(int amount)
        {
            _hitDamageAreaLevel += amount;
        }
        #endregion

        #region [CastSkill]
        void Scatter(GameObject origin, string prefabName, int level, int startIndex)
        {
            int scatterCount = (level == 0) ? 0 : (level * 2) + 1;

            for (int k = startIndex; k < scatterCount; k++)
            {
                //분산 각도
                float sign = ((k % 2 == 0) ? 1 : -1) * (Mathf.Ceil(k / 2.0f));
                Vector2 scatterDir = RotateVector(origin.transform.up, (sign * _scatterAngle)).normalized;

                GameObject castScatterCopy = YSJ.Managers.Pool.InstPrefab(prefabName);
                castScatterCopy.transform.position = origin.transform.position;
                castScatterCopy.transform.up = scatterDir;
                castScatterCopy.name = prefabName;
                castScatterCopy.GetComponent<Projectile>().SkillManager = this;
            }
        }

        IEnumerator CastScatterCoroutine(GameObject origin)
        {
            Scatter(origin, origin.name, _castScatterLevel, 1);
            yield return null;
        }

        IEnumerator CastAdditionalCoroutine(GameObject origin)
        {
            Vector3 startPos = origin.transform.position;
            
            for (int i = 0; i < _castAdditionalLevel; i++)
            {
                yield return new WaitForSeconds(0.05f);
                GameObject castAdditionalCopy = YSJ.Managers.Pool.InstPrefab(origin.name);
                castAdditionalCopy.transform.position = startPos;
                castAdditionalCopy.transform.up = origin.transform.up;
                castAdditionalCopy.name = origin.name;
                castAdditionalCopy.GetComponent<Projectile>().SkillManager = this;

                Scatter(castAdditionalCopy, origin.name, _castScatterLevel, 1);
            }
        }

        IEnumerator CastExplosionCoroutine(GameObject origin)
        {
            Vector3 startPos = origin.transform.position;

            for (int i = 0; i < _castExplosionLevel; i++)
            {
                GameObject castExplosion = YSJ.Managers.Pool.InstPrefab("Explosion");
                castExplosion.transform.position = startPos;
                yield return new WaitForSeconds(0.05f);
            }
        }

        IEnumerator CastDamageAreaCoroutine(GameObject origin)
        {
            Vector3 startPos = origin.transform.position;

            for (int i = 0; i < _castDamageAreaLevel; i++)
            {
                GameObject castDamageArea = YSJ.Managers.Pool.InstPrefab("DamageArea");
                castDamageArea.transform.position = startPos;
                yield return new WaitForSeconds(0.05f);
            }
        }
        #endregion

        #region [HitSkill]
        IEnumerator HitScatter(GameObject origin)
        {
            Scatter(origin, "HitScatter", _hitScatterLevel, 0);
            /*int scatterCount = (_hitScatterLevel == 0) ? 0 : ((_hitScatterLevel * 2) + 1);

            for (int k = 0; k < scatterCount; k++)
            {
                //분산 각도
                float sign = ((k % 2 == 0) ? 1 : -1) * (Mathf.Ceil(k / 2.0f));
                Vector2 scatterDir = RotateVector(origin.transform.up, (sign * _scatterAngle)).normalized;

                GameObject hitScatterCopy = YSJ.Managers.Pool.InstPrefab("HitScatter");
                hitScatterCopy.transform.position = origin.transform.position;
                hitScatterCopy.transform.up = scatterDir;
            }*/

            yield return null;
        }

        IEnumerator HitExplosionCoroutine(GameObject origin)
        {
            Vector3 startPos = origin.transform.position;

            for (int i = 0; i < _hitExplosionLevel; i++)
            {
                GameObject castExplosion = YSJ.Managers.Pool.InstPrefab("Explosion");
                castExplosion.transform.position = startPos;
                yield return new WaitForSeconds(0.05f);
            }
        }

        IEnumerator HitDamageAreaCoroutine(GameObject origin)
        {
            Vector3 startPos = origin.transform.position;

            for (int i = 0; i < _hitDamageAreaLevel; i++)
            {
                GameObject hitDamageArea = YSJ.Managers.Pool.InstPrefab("DamageArea");
                hitDamageArea.transform.position = startPos;
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