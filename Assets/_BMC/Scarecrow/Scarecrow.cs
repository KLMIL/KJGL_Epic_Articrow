using TMPro;
using UnityEngine;
using YSJ;

namespace BMC
{
    /// <summary>
    /// 허수아비, 플레이어가 방을 클리어 하면 나오는 허수아비
    /// </summary>
    public class Scarecrow : MonoBehaviour, IDamagable
    {
        Animator _anim;
        TextMeshPro _damageText;

        void Awake()
        {
            _anim = GetComponent<Animator>();
        }

        public void TakeDamage(float damage)
        {
            _anim.Play("Hurt");
            ShowDamageText(damage);
        }

        // 데미지 텍스트 띄우기
        void ShowDamageText(float damage)
        {
            /*TextMeshPro damageText = Managers.TestPool.Get<TextMeshPro>(Define.PoolID.DamageText);
            damageText.transform.position = transform.position;
            damageText.text = damage.ToString();*/

            // 대미지 부여 텍스트
            if (_damageText != null && _damageText.gameObject.activeInHierarchy)
            {
                _damageText.text = (float.Parse(_damageText.text) + damage).ToString();

                Color color = _damageText.color;
                color.a = 1;
                _damageText.color = color;
            }
            else
            {
                _damageText = Managers.TestPool.Get<TextMeshPro>(Define.PoolID.DamageText);
                _damageText.text = damage.ToString("F0");
            }
            _damageText.transform.position = this.transform.position + this.transform.up;
        }
    }
}