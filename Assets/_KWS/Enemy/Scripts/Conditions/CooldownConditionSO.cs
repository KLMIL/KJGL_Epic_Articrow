using UnityEngine;

[CreateAssetMenu(fileName = "CooldownCondition", menuName = "Enemy/Condition/Cooldown")]
public class CooldownConditionSO : EnemyConditionSO
{
    public string actionName;
    public override bool IsMet(EnemyController controller)
    {
        // 나중에 쿨다운 검사 로직 추가
        return true;
    }
}
