using System.Collections;
using UnityEngine;

namespace BMC
{
    public class SpikeAttackIndicator : MonoBehaviour
    {
        Coroutine _coroutine;

        ShokeWaveAttackIndicator[] _shokeWaveAttackIndicators;

        void Awake()
        {
            _shokeWaveAttackIndicators = GetComponentsInChildren<ShokeWaveAttackIndicator>();
        }

        public void Init(float time)
        {
            for (int i = 0; i < _shokeWaveAttackIndicators.Length; i++)
            {
                _shokeWaveAttackIndicators[i].Init(time);
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
                PlayChargeAndAttack();
        }

        public void PlayChargeAndAttack()
        {
            if (_coroutine == null)
                _coroutine = StartCoroutine(ChargeCoroutine());
        }

        private IEnumerator ChargeCoroutine()
        {
            //float dist = Vector2.Distance(_target.position, transform.position);
            float dist = 7f;

            float angle = 0f;
            float angleStep = 360f / _shokeWaveAttackIndicators.Length;

            for (int i = 0; i < _shokeWaveAttackIndicators.Length; i++)
            {
                angle = angleStep * i;
                Vector2 pos = GetPositionByAngle(transform.position, angle, dist);
                _shokeWaveAttackIndicators[i].transform.position = pos;
                _shokeWaveAttackIndicators[i].PlayChargeAndAttack();
            }

            yield break;
        }

        Vector2 GetPositionByAngle(Vector2 origin, float angleDeg, float distance)
        {
            float angleRad = angleDeg * Mathf.Deg2Rad;
            Vector2 offset = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * distance;
            return origin + offset;
        }
    }
}