using UnityEngine;

namespace BMC
{
    public class AdditionalDamage : PassiveSkill
    {
        void Awake()
        {
            _passiveSkillType = PassiveSkillType.AdditionalDamage;
            _amount = 10; // 추가 데미지
        }

        public override void Apply()
        {
            //PlayerManager.Instance.PlayerStatus.RightDamage += _amount;
            Debug.LogWarning($"추가 데미지 {_amount} 적용");
        }

        public override void Remove()
        {
            //PlayerManager.Instance.PlayerStatus.RightDamage -= _amount;
            Debug.LogWarning($"추가 데미지 {_amount} 해제");
        }
    }
}