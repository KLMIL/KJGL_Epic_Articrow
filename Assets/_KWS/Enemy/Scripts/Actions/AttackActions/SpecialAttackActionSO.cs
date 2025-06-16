using UnityEngine;

/*
 * 특수 공격들 정의
 * Summon: 프리펩에 해당하는 적 개체 소환 
 */
namespace Game.Enemy
{
    public enum SpecialAttackMode { Summon };
    [CreateAssetMenu(
        fileName = "SpecialAttackAction",
        menuName = "Enemy/Action/Attack/Special Attack"
    )]
    public class SpecialAttackActionSO : EnemyActionSO
    {
        public SpecialAttackMode specialAttackMode = SpecialAttackMode.Summon;
        public float cooldown = 1.0f;

        [Header("Summon")]
        public GameObject summoningPrefab;
        public int spawnCount = 3;
        public float spawnRadius = 1f;


        public override void Act(EnemyController controller)
        {
            // 현재 공격 상태의 쿨타임 체크
            string key = controller.CurrentStateName;
            if (!controller.lastAttackTimes.ContainsKey(key))
            {
                controller.lastAttackTimes[key] = -Mathf.Infinity;
            }

            if (Time.time - controller.lastAttackTimes[key] < cooldown) return;


            switch (specialAttackMode)
            {
                case SpecialAttackMode.Summon:
                    Summon(controller);
                    break;
            }

            // 쿨타임 부여
            controller.lastAttackTimes[key] = Time.time;
        }

        public override void OnEnter(EnemyController controller)
        {
            //controller.lastAttackTimes[controller.CurrentStateName] = -Mathf.Infinity;
        }

        public override void OnExit(EnemyController controller)
        {
            switch (specialAttackMode)
            {
                case SpecialAttackMode.Summon:
                    controller.FSM.isSpawnedMite = false;
                    break;
            }
        }


        private void Summon(EnemyController controller)
        {
            if (controller.FSM.isSpawnedMite) return;
            controller.FSM.isSpawnedMite = true;

            for (int i = 0; i < spawnCount; i++)
            {
                Vector2 rand = Random.insideUnitCircle * spawnRadius;
                Vector3 spawnPos = controller.transform.position + new Vector3(rand.x, 0, rand.y);
                Instantiate(summoningPrefab, spawnPos, Quaternion.identity);
            }
        }
    }
}