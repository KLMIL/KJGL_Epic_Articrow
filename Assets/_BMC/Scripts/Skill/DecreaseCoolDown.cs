using UnityEngine;

namespace BMC
{
    public class DecreaseCoolDown : PassiveSkill
    {
        void Awake()
        {
            _passiveSkillType = PassiveSkillType.IncreaseMaxMana;
            _amount = 0.1f; // 쿨타임 감소량
        }

        public override void Apply()
        {
            PlayerManager.Instance.PlayerStatus.RightCoolTime += -_amount;
            Debug.LogWarning("쿨타임 감소");
        }

        public override void Remove()
        {
            PlayerManager.Instance.PlayerStatus.RightCoolTime += _amount;
            Debug.LogWarning("쿨타임 증가");
        }
    }
}