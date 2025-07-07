using System.Collections;
using UnityEngine;

namespace BMC
{
    public class RushAttackIndicator : AttackIndicator
    {
        [Header("길이")]
        float _dist = 30f;

        void Awake()
        {
            SetSprite();
        }

        void Start()
        {
            _startScale = 0.5f;
            _endScale = 1.5f;
            _target = PlayerManager.Instance.transform;
        }

        //void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.T))
        //        PlayChargeAndAttack();
        //}

        protected override IEnumerator ChargeCoroutine()
        {
            // 목표를 바라보게 회전
            Vector3 dir = (_target.position - transform.position).normalized;
            transform.right = dir;

            // 원래 스케일 초기화
            _background.transform.localScale = new Vector3(0, _endScale, 1);
            _indicator.enabled = true;

            float timer = 0;
            while (timer < _fillDuration)
            {
                timer += Time.deltaTime;
                float r = timer / _fillDuration;
                float localScaleY = Mathf.Lerp(_startScale, _endScale, r);
                yield return null;
            }

            _indicator.enabled = false;
            yield return new WaitForSeconds(0.1f);
            _coroutine = null;
        }
    }
}