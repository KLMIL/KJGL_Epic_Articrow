using BMC;
using System;
using System.Collections;
using UnityEngine;

namespace CKT
{
    [System.Serializable]
    public abstract class Projectile : MonoBehaviour
    {
        protected bool _isCreateFromPlayer;
        protected ArtifactSO _artifactSO;

        protected int _penetration;

        public virtual void Init(bool isCreateFromPlayer)
        {
            _isCreateFromPlayer = isCreateFromPlayer;
            _artifactSO = GameManager.Instance.RightSkillManager.GetArtifactSOFuncT0.Trigger();

            _penetration = (isCreateFromPlayer) ? _artifactSO.Penetration : _artifactSO.Penetration + 1;

            float existTime = _artifactSO.ExistTime - PlayerManager.Instance.PlayerStatus.RightCoolTime;
            existTime = Mathf.Clamp(existTime, 0, float.MaxValue);
            _disableCoroutine = StartCoroutine(DisableCoroutine(existTime));

            _moveCoroutine = StartCoroutine(MoveCoroutine(_artifactSO.MoveSpeed));
        }

        protected Coroutine _disableCoroutine;
        protected Coroutine _moveCoroutine;
        Transform _target;

        protected void OnDisable()
        {
            if (_disableCoroutine != null)
            {
                StopCoroutine(_disableCoroutine);
                _disableCoroutine = null;
            }
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
                _moveCoroutine = null;
            }

            _target = null;
            _artifactSO = null;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.isTrigger) return;
            //if (collision.isTrigger || _target != null) return;

            _target = collision.transform;
            IDamagable iDamageable = _target.GetComponent<IDamagable>();
            if (iDamageable != null)
            {
                float totalDamage = _artifactSO.Damage + PlayerManager.Instance.PlayerStatus.RightDamage;
                float damageRate = GameManager.Instance.RightSkillManager.DamageRate;
                iDamageable.TakeDamage(totalDamage * damageRate);

                //true면 플레이가 호출한 Projectile,  false면 HitSkill에서 생성된 Projectile
                if (_isCreateFromPlayer)
                {
                    Vector3 closestPoint = collision.ClosestPoint(YSJ.Managers.Input.MouseWorldPos);
                    SkillManager skillManager = GameManager.Instance.RightSkillManager;
                    foreach (Func<Vector3, Vector3, IEnumerator> hitSkill in skillManager.HitSkillDict.Values)
                    {
                        StartCoroutine(hitSkill(closestPoint, this.transform.up));
                    }
                }

                _penetration--;
                if (_penetration < 0)
                {
                    //다음 프레임에 비활성화
                    if (_disableCoroutine != null)
                    {
                        StopCoroutine(_disableCoroutine);
                    }
                    _disableCoroutine = StartCoroutine(DisableCoroutine(0));

                    //이동 정지
                    if (_moveCoroutine != null)
                    {
                        StopCoroutine(_moveCoroutine);
                        _moveCoroutine = null;
                    }
                }
            }
        }

        protected virtual IEnumerator DisableCoroutine(float existTime)
        {
            yield return (existTime <= 0) ? null : new WaitForSeconds(existTime);
            YSJ.Managers.TestPool.Return(_artifactSO.ProjectilePoolID, this.gameObject);
            _disableCoroutine = null;
        }

        IEnumerator MoveCoroutine(float moveSpeed)
        {
            while (this.gameObject.activeSelf)
            {
                transform.position += transform.up * moveSpeed * Time.deltaTime;
                yield return null;
            }

            _moveCoroutine = null;
        }
    }
}