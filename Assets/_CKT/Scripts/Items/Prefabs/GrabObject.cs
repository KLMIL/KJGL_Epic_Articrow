using System.Collections;
using UnityEngine;

namespace CKT
{
    public class GrabObject : MonoBehaviour
    {
        int _level = 0;
        float _grabScale = 8f;
        float _grabTime = 0.15f;
        float _minDistance = 1f;

        Transform _target;
        Coroutine _disableCoroutine;

        private void OnEnable()
        {
            _disableCoroutine = StartCoroutine(DisableCoroutine(0.2f));
        }

        private void OnDisable()
        {
            _level = 0;
            _target = null;
        }

        public void Init(int level)
        {
            _level = level;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.isTrigger) return;

            IDamagable iDamagable = collision.transform.GetComponent<IDamagable>();
            GrabObject grabObject = collision.transform.GetComponentInParent<GrabObject>();
            if ((iDamagable != null) && (grabObject == null))
            {
                _target = collision.transform;
                //당겨오기
                StartCoroutine(MoveCoroutine(_target));
                //적중 못했을 때 비활성화 타이머 끄기
                StopCoroutine(_disableCoroutine);

                //TODO : 사운드_HitGrab
            }
        }

        IEnumerator MoveCoroutine(Transform target)
        {
            this.transform.position = target.position;
            target.SetParent(this.transform);
            Vector3 startPos = target.position;

            Transform player = FindAnyObjectByType<BMC.PlayerManager>().transform;
            Vector3 playerPos = player.position;

            if ((player != null) && (target != null))
            {
                float distance = 0;
                while (distance < (_grabTime * _level))
                {
                    distance += Time.deltaTime;

                    float sqrDistance = (playerPos - this.transform.position).sqrMagnitude;
                    if (sqrDistance > (_minDistance * _minDistance))
                    {
                        Vector3 moveDir = (playerPos - startPos).normalized;
                        transform.position += moveDir * _grabScale * Time.deltaTime;
                    }

                    yield return null;
                }

                target.SetParent(null);
            }

            YSJ.Managers.TestPool.Return(Define.PoolID.GrabObject, this.gameObject);
        }

        IEnumerator DisableCoroutine(float waitTime)
        {
            yield return (waitTime <= 0) ? null : new WaitForSeconds(waitTime);
            YSJ.Managers.TestPool.Return(Define.PoolID.GrabObject, this.gameObject);
        }
    }
}