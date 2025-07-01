using TMPro;
using UnityEngine;
using YSJ;

namespace BMC
{
    /// <summary>
    /// 데미지 텍스트 띄우는 클래스
    /// </summary>
    public class ShowDamageText : MonoBehaviour
    {
        TextMeshPro _damageText;

        void Awake()
        {
            _damageText = Managers.TestPool.Get<TextMeshPro>(Define.PoolID.DamageText);
        }

        // 데미지 텍스트 띄우기
        public void Show(float damage)
        {
            // 대미지 부여 텍스트
            if (_damageText != null && _damageText.gameObject.activeInHierarchy)
            {
                _damageText.text = (float.Parse(_damageText.text) + damage).ToString();

                Color color = _damageText.color;
                color.a = 1;
                _damageText.color = color;
            }

            _damageText.transform.position = this.transform.position + this.transform.up;
        }

        void OnDestroy()
        {
            if (_damageText != null)
            {
                Managers.TestPool.Return(Define.PoolID.DamageText, _damageText.gameObject);
                _damageText = null;
            }
        }
    }
}