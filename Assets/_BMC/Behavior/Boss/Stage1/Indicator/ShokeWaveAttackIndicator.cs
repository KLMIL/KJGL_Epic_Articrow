using System.Collections;
using UnityEngine;
namespace BMC
{
    public class ShokeWaveAttackIndicator : AttackIndicator
    {
        CapsuleCollider2D _capsuleCollider;

        ShokeWaveHitBox _shokeWaveHitBox; // 히트박스 컴포넌트

        public static int sfxPlayCount;

        [SerializeField] GameObject _spikePrefab;

        void Awake()
        {
            SetSprite();
            _capsuleCollider = GetComponentInChildren<CapsuleCollider2D>();
            _shokeWaveHitBox = GetComponentInChildren<ShokeWaveHitBox>();
        }

        void Start()
        {
            _startScale = 0.1f;
            _endScale = 1f;
            _shokeWaveHitBox.Damage = 2f;
        }

        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.V))
            //    PlayChargeAndAttack();
        }

        protected override IEnumerator ChargeCoroutine()
        {
            // 원래 스케일 초기화
            _indicator.transform.localScale = new Vector3(_startScale, _startScale, 1);
            _background.transform.localScale = new Vector3(_endScale, _endScale, 1);
            _indicator.enabled = true;
            _background.enabled = true;

            // 배경에 딱 채우기
            float timer = 0;
            while (timer < _fillDuration)
            {
                timer += Time.deltaTime;
                float r = timer / _fillDuration;
                float localScaleX = Mathf.Lerp(_startScale, _endScale, r);
                float localScaleY = Mathf.Lerp(_startScale, _endScale, r);
                _indicator.transform.localScale = new Vector3(localScaleX, localScaleY, 1);
                yield return null;
            }
            _indicator.transform.localScale = new Vector3(_endScale, _endScale, 1);

            // 공격 발동
            if (sfxPlayCount == 0)
            {
                sfxPlayCount++;
                YSJ.Managers.Sound.PlaySFX(Define.SFX.GolemSpike);
            }

            GameObject spikeInstance = Instantiate(_spikePrefab, _indicator.transform.position, Quaternion.identity);
            _background.enabled = false;
            _indicator.enabled = true;
            _capsuleCollider.enabled = true;
            Color originalColor = _indicator.color;
            _indicator.color = Color.white;
            
            yield return new WaitForSeconds(0.1f);
            _capsuleCollider.enabled = false;
            _indicator.enabled = false;
            _indicator.color = originalColor;
            _coroutine = null;
            _indicator.transform.localScale = new Vector3(_startScale, _startScale, 1);
            _background.transform.localScale = new Vector3(_endScale, _endScale, 1);
        }
    }
}