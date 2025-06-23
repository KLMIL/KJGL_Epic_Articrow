using System.Collections;
using UnityEngine;

namespace BMC
{
    public class EnemyAttackIndicator : MonoBehaviour
    {
        Transform _target;               // 공격 목표
        SpriteRenderer _indicator;             // 인디케이터
        SpriteRenderer _background;            // 배경
        Coroutine _coroutine;

        [Header("길이")]
        float _dist = 30f;

        [Header("시간")]
        float _fillDuration = 1f;   // 두께 확장 시간

        [Header("두께")]
        float _startThickness = 0.5f; // 시작 두께
        float _endThickness = 1f;   // 최종 두께


        void Awake()
        {
            SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            _indicator = spriteRenderers[0];
            _background = spriteRenderers[1];
            _indicator.enabled = false;
            _background.enabled = false;
        }

        void Start()
        {
            _target = PlayerManager.Instance.transform;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
                PlayChargeAndAttack();
        }

        public void Init(float time)
        {
            _fillDuration = time;
        }

        public void PlayChargeAndAttack()
        {
            if(_coroutine == null)
                _coroutine = StartCoroutine(ChargeCoroutine());
        }

        private IEnumerator ChargeCoroutine()
        {
            // 방향을 목표로 회전
            Vector3 dir = (_target.position - transform.position).normalized;
            transform.right = dir;

            // 원래 스케일 초기화
            _indicator.transform.localScale = new Vector3(0, _startThickness, 1);
            _background.transform.localScale = new Vector3(0, _endThickness, 1);
            _indicator.enabled = true;
            _background.enabled = true;

            //_dist = Vector2.Distance(_indicator.transform.position, _target.position) / 2;

            // 1. 길이 확장
            _indicator.transform.localScale = new Vector3(_dist, _endThickness, 1);
            _background.transform.localScale = new Vector3(_dist, _endThickness, 1);

            // 2. 배경에 딱 채우기
            float timer = 0;
            while (timer < _fillDuration)
            {
                timer += Time.deltaTime;
                float r = timer / _fillDuration;
                float localScaleY = Mathf.Lerp(_startThickness, _endThickness, r);
                _indicator.transform.localScale = new Vector3(_dist, localScaleY, 1);
                yield return null;
            }
            _indicator.transform.localScale = new Vector3(_dist, _endThickness, 1);

            _indicator.enabled = false;
            _background.enabled = false;
            yield return new WaitForSeconds(0.1f);
            _coroutine = null;
            // 공격 실행
            // TODO: 여기에 공격을 하라고 하든 해야함
        }
    }
}