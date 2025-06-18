using UnityEngine;

namespace BMC
{
    public enum PassiveSkillType
    {
        AdditionalDamage, // 추가 피해
        IncreaseMaxMana,  // 최댐 마나 증가
        DecreaseSpendMana,// 마나 소모 감소
        DecreaseCoolDown, // 쿨타임 감소
    }

    // 패시브 스킬이 상속받아야 할 클래스
    public class PassiveSkill : MonoBehaviour
    {
        protected PassiveSkillType _passiveSkillType; // 패시브 스킬의 종류
        protected int _amount; // 스킬 효과의 양

        // 스킬 효과 적용
        public virtual void Apply()
        { }

        // 스킬 효과 제거
        public virtual void Remove()
        { }
    }
}