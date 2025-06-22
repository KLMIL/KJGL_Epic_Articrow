using TMPro;
using Unity.Behavior;
using UnityEngine;
using YSJ;

namespace BMC
{
    /// <summary>
    /// 보스들이 가지는 기본적인 상태 정보
    /// </summary>
    public class BossStatus : MonoBehaviour
    {
        protected BehaviorGraphAgent _behaviorGraphAgent;                      // behavior graph 에이전트
        protected TextMeshPro _damageText;                                     // 데미지 텍스트
        protected SpriteRenderer _visual;                                      // 외관
        protected WaitForSeconds _colorChangeTime = new WaitForSeconds(0.25f); // 피격 시 색상 변경 시간

        [Header("상태")]
        [field: SerializeField] public bool IsDead { get; set; }

        [Header("일반 스테이터스")]
        [field: SerializeField] public float Health { get; set; }

        // 보스 상태 초기화
        public virtual void Init()
        {
        }

        // 받은 데미지 보여주기
        public void ShowTakeDamageText(float damage)
        {
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
                _damageText.text = damage.ToString();
            }
            _damageText.transform.position = this.transform.position + this.transform.up * 1.5f;
        }

        // 사망
        public virtual void Die()
        {
        }
    }
}