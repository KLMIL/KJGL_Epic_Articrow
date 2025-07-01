using UnityEngine;

namespace BMC
{
    public class DecreaseSpendMana : PassiveSkill
    {
        void Awake()
        {
            _passiveSkillType = PassiveSkillType.IncreaseMaxMana;
            _amount = 5; // 마나 소모량 감소폭
        }

        public override void Apply()
        {
            //PlayerManager.Instance.PlayerStatus.SpendManaOffsetAmount += _amount;
            Debug.LogWarning($"마나 소모량 감소 {_amount}");
        }

        public override void Remove()
        {
            //PlayerManager.Instance.PlayerStatus.SpendManaOffsetAmount -= _amount;
            Debug.LogWarning($"마나 소모량 증가 {_amount}");
        }
    }
}