using System.Collections;
using UnityEngine;

namespace BMC
{
    public class AttackIndicator : MonoBehaviour
    {
        protected Transform _target;                // 공격 목표
        protected SpriteRenderer _indicator;        // 인디케이터
        protected SpriteRenderer _background;       // 배경
        protected Coroutine _coroutine;

        [Header("시간")]
        protected float _fillDuration = 1f;         // 확장 시간

        [Header("크기")]
        protected float _startScale = 0.5f;         // 시작 크기
        protected float _endScale = 1.5f;           // 최종 크기

        void Start()
        {
            _target = PlayerManager.Instance.transform;
        }

        public virtual void Init(float fillDuration)
        {
            _fillDuration = fillDuration;
        }

        public void SetSprite()
        {
            SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            _indicator = spriteRenderers[0];
            _background = spriteRenderers[1];
            _indicator.enabled = false;
            _background.enabled = false;
        }

        public virtual void PlayChargeAndAttack()
        {
            if (_coroutine == null)
                _coroutine = StartCoroutine(ChargeCoroutine());
        }

        protected virtual IEnumerator ChargeCoroutine()
        {
            yield break; // 기본 구현은 아무것도 하지 않음
        }
    }
}