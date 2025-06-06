using UnityEngine;

[CreateAssetMenu(fileName = "CooldownCondition", menuName = "Enemy/Condition/Cooldown")]
public class CooldownConditionSO : EnemyConditionSO
{
    public string behaviourName;
    public override bool IsMet(EnemyController controller)
    {
        // 등록되지 않았다면 true로 취급
        if (!controller.lastAttackTimes.ContainsKey(behaviourName)) return true;


        float lastTime = controller.lastAttackTimes[behaviourName];
        float cooldown = 0f;

        var behaviour = controller.Behaviours.Find(b => b.stateName == behaviourName);
        if (behaviour.action is MeleeAttackActionSO melee)
        {
            cooldown = melee.cooldown;
        }
        else if (behaviour.action is ProjectileAttackActionSO proj)
        {
            cooldown = proj.cooldown;
        }
        else if (behaviour.action is SpecialAttackActionSO special)
        {
            cooldown = special.cooldown;
        }
        // (필요시 Action에서 쿨타임 얻어오는 로직 추가)

        
        return (Time.time - lastTime) >= cooldown;
    }
}
