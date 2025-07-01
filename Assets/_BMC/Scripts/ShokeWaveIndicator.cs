using System.Collections;
using UnityEngine;
namespace BMC
{
    public class ShokeWaveIndicator : MonoBehaviour
    {
        Transform _target;                     // 공격 목표
        SpriteRenderer _indicator;             // 인디케이터
        SpriteRenderer _background;            // 배경
        Coroutine _coroutine;
        CircleCollider2D _circleCollider;

        [Header("시간")]
        float _fillDuration = 1f;   // 두께 확장 시간

        [Header("두께")]
        float _startThickness = 0.5f;   // 시작 두께
        float _endThickness = 5f;       // 최종 두께


        void Awake()
        {
            SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            _indicator = spriteRenderers[0];
            _background = spriteRenderers[1];
            _indicator.enabled = false;
            _background.enabled = false;

            _circleCollider = GetComponentInChildren<CircleCollider2D>();
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
            if (_coroutine == null)
                _coroutine = StartCoroutine(ChargeCoroutine());
        }

        private IEnumerator ChargeCoroutine()
        {
            // 원래 스케일 초기화
            _indicator.transform.localScale = new Vector3(_startThickness, _startThickness, 1);
            _background.transform.localScale = new Vector3(_endThickness, _endThickness, 1);
            _indicator.enabled = true;
            _background.enabled = true;

            // 배경에 딱 채우기
            float timer = 0;
            while (timer < _fillDuration)
            {
                timer += Time.deltaTime;
                float r = timer / _fillDuration;
                float localScaleX = Mathf.Lerp(_startThickness, _endThickness, r);
                float localScaleY = Mathf.Lerp(_startThickness, _endThickness, r);
                _indicator.transform.localScale = new Vector3(localScaleX, localScaleY, 1);
                yield return null;
            }
            _indicator.transform.localScale = new Vector3(_endThickness, _endThickness, 1);

            _background.enabled = false;
            _indicator.enabled = true;
            _circleCollider.enabled = true;
            Color originalColor = _indicator.color;
            _indicator.color = Color.white;
            
            yield return new WaitForSeconds(0.1f);

            _circleCollider.enabled = false;
            _indicator.enabled = false;
            _indicator.color = originalColor;
            _coroutine = null;
        }
    }
}