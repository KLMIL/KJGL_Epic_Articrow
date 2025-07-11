using System.Collections;
using TMPro;
using UnityEngine;
using YSJ;
using static UnityEngine.Rendering.DebugUI;

namespace BMC
{
    /// <summary>
    /// 데미지 텍스트 띄우는 클래스
    /// </summary>
    public class ShowDamageText : MonoBehaviour
    {
        //TextMeshPro _damageText;
        float TotalDamage
        {
            get
            {
                return _totalDamage;
            }
            set
            {
                _totalDamage = value;
                _totalDamage = 0.01f * Mathf.RoundToInt(_totalDamage * 100f);
            }
        }
        float _totalDamage = 0;
        float _waitTime = 0.06f;
        Coroutine _waitDamageCoroutine = null;

        void Awake()
        {
            /*_damageText = Managers.Pool.Get<TextMeshPro>(Define.PoolID.DamageText);
            
            Color color = _damageText.color;
            color.a = 0;
            _damageText.color = color;*/
        }

        void OnDestroy()
        {
            /*if (_damageText != null)
            {
                Managers.Pool.Return(Define.PoolID.DamageText, _damageText.gameObject);
                _damageText = null;
            }*/
        }

        // 데미지 텍스트 띄우기
        public void Show(float damage)
        {
            // 대미지 부여 텍스트
            /*if (_damageText != null && _damageText.gameObject.activeInHierarchy)
            {
                _damageText.text = $"{damage}";

                Color color = _damageText.color;
                color.a = 1;
                _damageText.color = color;
            }
            _damageText.transform.position = this.transform.position + this.transform.up;*/

            TotalDamage += damage;
            _waitDamageCoroutine = _waitDamageCoroutine ?? StartCoroutine(WaitDamageCoroutine());
        }

        /// <summary>
        /// 한 프레임 안에 들어온 텍스트를 더해서 한 번에 보여주는 용도
        /// </summary>
        /// <returns></returns>
        IEnumerator WaitDamageCoroutine()
        {
            yield return (_waitTime <= 0) ? null : new WaitForSeconds(_waitTime);
            
            TextMeshPro damageText = Managers.Pool.Get<TextMeshPro>(Define.PoolID.DamageText);

            //텍스트 표시 + 합산 데미지 초기화
            if (Mathf.Approximately(TotalDamage, Mathf.Round(TotalDamage)))
            {
                damageText.text = $"{Mathf.RoundToInt(TotalDamage)}";
            }
            else
            {
                damageText.text = $"{TotalDamage:F1}";
            }
            TotalDamage = 0;
            
            //투명도 초기화
            Color color = damageText.color;
            color.a = 1;
            damageText.color = color;
            
            //위치 설정
            damageText.transform.position = this.transform.position + this.transform.up;

            _waitDamageCoroutine = null;
        }
    }
}