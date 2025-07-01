using System.Collections.Generic;
using System.Linq;
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

            // 후보 필터링
            var candidates = patternCandidates
                .Where(p =>
                    Time.time - p.lastUsedTime >= p.cooldown &&
                    currentHpRatio <= p.hpUnlockRatio &&
                    (p.canRepeat || lastPattern == null || p.patternStateName != lastPattern)
                ).ToList();

            // 후보가 없는 경우 -> Idle
            if (candidates.Count == 0)
            {
                controller.FSM.ChangeState("Idle");
                return;
            }

            // 패턴 가중치 랜덤 선택
            float totalWeight = candidates.Sum(p => p.probability);
            float r = UnityEngine.Random.value * totalWeight;
            float acc = 0f;

            foreach (var p in candidates)
            {
                acc += p.probability;
                if (r <= acc)
                {
                    lastPattern = p.patternStateName;
                    p.lastUsedTime = Time.time;

                    controller.FSM.ChangeState(p.patternStateName);
                    return;
                }
            }
        }
    }
}
