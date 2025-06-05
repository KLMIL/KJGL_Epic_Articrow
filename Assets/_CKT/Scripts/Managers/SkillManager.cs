using System;
using System.Collections;
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

        int _hitScatterLevel;
        int _hitExplosionLevel;

        #region [Action, Func]
        event Action<GameObject> _onCastSkillEvent;
        void SubCastSkillEvent(Action<GameObject> newSub)
        {
            _onCastSkillEvent += newSub;
        }
        public void InvokeCastSkillEvent(GameObject obj)
        {
            _onCastSkillEvent?.Invoke(obj);
        }

        event Func<GameObject, IEnumerator> _getCastSkillIEnumerator;
        void SubCastSkillIEnumerator(Func<GameObject, IEnumerator> newSub)
        {
            _getCastSkillIEnumerator += newSub;
        }
        public IEnumerator InvokeCastSkillIEnumerator(GameObject obj)
        {
            return _getCastSkillIEnumerator?.Invoke(obj);
        }

        //===========================================================

        event Action<GameObject> _onHitSkillEvent;
        void SubHitSkillEvent(Action<GameObject> newSub)
        {
            _onHitSkillEvent += newSub;
        }
        public void InvokeHitSkillEvent(GameObject obj)
        {
            _onHitSkillEvent?.Invoke(obj);
        }

        event Func<GameObject, IEnumerator> _getHitSkillIEnumerator;
        void SubHitSkillIEnumerator(Func<GameObject, IEnumerator> newSub)
        {
            _getHitSkillIEnumerator += newSub;
        }
        public IEnumerator InvokeHitSkillIEnumerator(GameObject obj)
        {
            return _getHitSkillIEnumerator?.Invoke(obj);
        }
        #endregion

        #region [Inventory에서 호출할 함수]
        public void InitLevel()
        {
            _scatterAngle = 3f;

            _castScatterLevel = 0;
            _castAdditionalLevel = 0;
            _castExplosionLevel = 0;

            _hitScatterLevel = 0;
            _hitExplosionLevel = 0;

            //CastSkill
            _onCastSkillEvent = null;
            SubCastSkillEvent((obj) => CastScatter(obj));

            _getCastSkillIEnumerator = null;
            SubCastSkillIEnumerator((obj) => CastAdditionalCoroutine(obj));
            SubCastSkillIEnumerator((obj) => CastExplosionCoroutine(obj));

            //HitSkill
            _onHitSkillEvent = null;
            SubHitSkillEvent((obj) => HitScatter(obj));

            _getHitSkillIEnumerator = null;
            SubHitSkillIEnumerator((obj) => HitExplosionCoroutine(obj));
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

        public void HitExplosionLevelUp(int amount)
        {
            _hitExplosionLevel += amount;
        }
        #endregion

        #region [CastSkill]
        void CastScatter(GameObject origin)
        {
            int scatterCount = _castScatterLevel * 2;

            for (int k = 0; k < scatterCount; k++)
            {
                //분산 각도
                float sign = ((k % 2 == 0) ? 1 : -1) * (Mathf.Floor(k / 2.0f) + 1);
                Vector2 scatterDir = RotateVector(origin.transform.up, (sign * _scatterAngle)).normalized;

                GameObject castScatterCopy = YSJ.Managers.Pool.InstPrefab(origin.name);
                castScatterCopy.transform.position = origin.transform.position;
                castScatterCopy.transform.up = scatterDir;
                castScatterCopy.name = origin.name;
            }
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
                CastScatter(castAdditionalCopy);
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
        #endregion

        #region [HitSkill]
        void HitScatter(GameObject origin)
        {
            int scatterCount = (_hitScatterLevel == 0) ? 0 : ((_hitScatterLevel * 2) + 1);

            for (int k = 0; k < scatterCount; k++)
            {
                //분산 각도
                float sign = ((k % 2 == 0) ? 1 : -1) * (Mathf.Ceil(k / 2.0f));
                Vector2 scatterDir = RotateVector(origin.transform.up, (sign * _scatterAngle)).normalized;

                GameObject hitScatterCopy = YSJ.Managers.Pool.InstPrefab("HitScatter");
                hitScatterCopy.transform.position = origin.transform.position;
                hitScatterCopy.transform.up = scatterDir;
            }
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