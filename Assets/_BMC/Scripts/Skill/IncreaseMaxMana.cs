using UnityEngine;

namespace BMC
{
    public class IncreaseMaxMana : PassiveSkill
    {
        void Awake()
        {
            _passiveSkillType = PassiveSkillType.IncreaseMaxMana;
            _amount = 10; // 마나 소모 감소량
        }

        public override void Apply()
        {
            PlayerManager.Instance.PlayerStatus.MaxMana += _amount;
            Debug.LogWarning("최대 마나 증가");
        }

        public override void Remove()
        {
            PlayerManager.Instance.PlayerStatus.MaxMana -= _amount;
            Debug.LogWarning("최대 마나 감소");
        }
    }
}