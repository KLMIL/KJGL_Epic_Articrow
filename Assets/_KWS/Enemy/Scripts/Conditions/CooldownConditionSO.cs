using UnityEngine;

namespace Game.Enemy
{
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
            else if (behaviour.action is DecidePatternActionSO decide)
            {
                // Decide Pattern Action 자체는 쿨타임이 없는 동작.
                // 최초 동작 수행 시간만을 통제하기 위해, SpawnedAction에서 변수값 가져와서 초기화.
                EnemyBehaviourUnit spawnedBehaviour = controller.Behaviours.Find(b => b.stateName == "Spawned");
                if (spawnedBehaviour != null && spawnedBehaviour.action is SpawnedActionSO spawnedAction)
                {
                    cooldown = spawnedAction.spawnAttackCooldown;
                }
            }
            // (필요시 Action에서 쿨타임 얻어오는 로직 추가)


            return (Time.time - lastTime) >= cooldown;
        }
    }
}
