using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;

/*
 * 지정된 패턴 중, 랜덤으로 선택
 */
namespace Game.Enemy
{
    [CreateAssetMenu(
        fileName = "DecidePatternAction",
        menuName = "Enemy/Action/State/Decide Pattern"
    )]
    [System.Serializable] // Candidates List 편집하려면 필요
    public class DecidePatternActionSO : EnemyActionSO
    {
        [SerializeField] List<EnemyPatternCandidate> patternCandidates = new();
        string lastPattern = null;

        public override void Act(EnemyController controller)
        {
            controller.FSM.isSuperArmor = true;

            float currentHpRatio = controller.Status.healthPoint / controller.StatusOrigin.healthPoint;

            // 가능한 행동 후보 필터링
            var candidates = patternCandidates
                .Where(p =>
                    currentHpRatio <= p.hpUnlockRatio &&
                    (p.canRepeat || lastPattern == null || p.patternStateName != lastPattern)
                ).ToList();

            // 후보 중 쿨타임이 끝난 행동만 남김
            var readyCandidates = candidates
                .Where(p =>
                {
                    string cooldownKey = GetCooldownKey(p.patternStateName);

                    var behaviour = controller.Behaviours.Find(b => b.stateName == cooldownKey);
                    if (behaviour == null) return false;

                    float cooldown = 0f;
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
                    // 기타 행동 추가

                    if (!controller.lastAttackTimes.ContainsKey(cooldownKey))
                    {
                        return false;
                    }
                    float lastTime = controller.lastAttackTimes[cooldownKey];


                    return (Time.time - lastTime) >= cooldown;
                    
                }).ToList();

            // 후보가 없는 경우 -> Idle
            if (readyCandidates.Count == 0)
            {
                controller.FSM.ChangeState("Idle");
                return;
            }

            // 패턴 가중치 랜덤 선택
            float totalWeight = readyCandidates.Sum(p => p.probability);
            float r = Random.value * totalWeight;
            float acc = 0f;

            foreach (var p in readyCandidates)
            {
                acc += p.probability;
                if (r <= acc)
                {
                    lastPattern = p.patternStateName;
                    controller.lastAttackTimes[p.patternStateName] = Time.time;

                    controller.FSM.ChangeState(p.patternStateName);
                    return;
                }
            }
        }

        string GetCooldownKey(string patternStateName)
        {
            if (patternStateName.EndsWith("Ready"))
            {
                return patternStateName.Replace("Ready", "");
            }
            return patternStateName;
        }
    }
}
