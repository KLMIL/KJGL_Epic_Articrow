using UnityEngine;

/*
 * 두 가지 Condition이 모두 만족될 때를 검사하기 위한 Condition
 */

namespace Game.Enemy
{
    [CreateAssetMenu(
        fileName = "And",
        menuName = "Enemy/Condition/AndCondition"
    )]
    public class AndConditionSO : EnemyConditionSO
    {
        [SerializeField] EnemyConditionSO[] conditions;

        public override bool IsMet(EnemyController controller)
        {
            if (conditions == null || conditions.Length == 0)
            {
                return false; // 아무 조건도 없다면 false
            }

            foreach (var condition in conditions)
            {
                if (condition == null)
                {
                    continue;
                }
                if (!condition.IsMet(controller))
                {
                    return false; // 하나라도 false면 전체 false
                }
            }
            return true; // 모두 true면 전체 true
        }
    }
}