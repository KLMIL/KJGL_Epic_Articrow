using UnityEngine;
using UnityEngine.InputSystem;

/*
 * 물리 공격(몬스터가 직접 근접해서 공격하는) 정의
 * Contact: 몸통박치기
 * Basic: 일반 공격
 * Rush: 돌진 공격
 */
namespace Game.Enemy
{
    public enum MeleeAttackMode { Basic, Rush, Smash }
    [CreateAssetMenu(
        fileName = "MeleeAttackAction",
        menuName = "Enemy/Action/Attack/Melee Attack"
    )]
    public class MeleeAttackActionSO : EnemyActionSO
    {
        public MeleeAttackMode meleeAttackMode = MeleeAttackMode.Basic;

        [Header("Common Field: Contact Attack")]
        public float damageMultiply = 1.0f;
        public float knockbackMultiply = 1.0f;
        public float cooldown = 1.0f;


        [Header("Basic Attack Only")]
        public float attackRange = 1.0f;
        public Vector2 attackOffset = Vector2.zero;

        [Header("Rush Attack Only")]
        public float rushSpeedMultiply = 6f;
        public bool trackingRush = false;
        public float rushDuration = 0;
        public bool inverse = false;

        [Header("Cone Attack Only")]
        public float coneRadius = 3f;
        public float coneAngle = 120f;
        public GameObject attackEffect;

        public override void Act(EnemyController controller)
        {
            controller.FSM.isSuperArmor = true;

            // 현재 공격 상태의 쿨타임 체크
            string key = controller.CurrentStateName;
            if (!controller.lastAttackTimes.ContainsKey(key))
            {
                controller.lastAttackTimes[key] = -Mathf.Infinity;
            }

            if (Time.time - controller.lastAttackTimes[key] < cooldown)
            {
                //Debug.Log($"Rest Cooltime: {Time.time - controller.lastAttackTimes[key]}");
                return;
            }

            // 공격 처리
            switch (meleeAttackMode)
            {
                //case MeleeAttackMode.Contact:
                //    ContactAttack(controller);
                //break;
                case MeleeAttackMode.Basic:
                    BasicAttack(controller);
                    break;
                case MeleeAttackMode.Rush:
                    RushAttack(controller);
                    break;
                case MeleeAttackMode.Smash:
                    SmashAttack(controller);
                    break;
            }


            // 쿨타임 부여
            controller.lastAttackTimes[key] = Time.time;
            //Debug.LogError("물리공격 쿨타임 부여");
        }

        public override void OnEnter(EnemyController controller)
        {
            if (meleeAttackMode == MeleeAttackMode.Rush)
            {
                controller.FSM.rushSpeedMultiply = this.rushSpeedMultiply;
                controller.FSM.currentActionDamageMultiply = this.damageMultiply;
            }
        }

        public override void OnExit(EnemyController controller)
        {
            switch (meleeAttackMode)
            {
                case MeleeAttackMode.Basic:
                    break;
                case MeleeAttackMode.Rush:
                    controller.FSM.isRushing = false;
                    controller.FSM.isRushAttacked = true;
                    controller.FSM.currentActionDamageMultiply = 1.0f;
                    break;
            }
        }


        private void BasicAttack(EnemyController controller)
        {
            Vector2 attackOrigin = (Vector2)controller.transform.position + attackOffset;
            float radius = attackRange;
            LayerMask playerMask = LayerMask.GetMask("PlayerHurtBox");

            Collider2D[] hits = Physics2D.OverlapCircleAll(attackOrigin, radius, playerMask);

            foreach (var hit in hits)
            {
                // 태그까지 검사 -> 안전망 역할

                    float damage = controller.Status.attack * damageMultiply;
                    Transform target = hit.transform;
                    controller.DealDamageToPlayer(damage, target, false);
                    break;
    
            }
        }

        private void RushAttack(EnemyController controller)
        {
            if (!controller.FSM.isRushing)// 일반 돌진
            {
                controller.FSM.rushDirection = (controller.FSM.AttackTargetPosition - (Vector2)controller.transform.position).normalized;
            }

            controller.FSM.rushDamageMultuply = damageMultiply;
            controller.FSM.isRushAttacked = false;
            controller.FSM.isRushing = true;
            controller.MoveTo(controller.FSM.rushDirection, rushDuration, "RushAttack");
        }

        private void SmashAttack(EnemyController controller)
        {
            Vector2 attackOrigin = (Vector2)controller.transform.position + attackOffset;
            float radius = coneRadius;
            float maxAngle = coneAngle * 0.5f;
            LayerMask playerMask = LayerMask.GetMask("PlayerHurtBox");
            LayerMask obstacleMask = LayerMask.GetMask("Obstacle");

            Vector2 forward = (controller.FSM.AttackTargetPosition - (Vector2)controller.transform.position).normalized;

            Collider2D[] hits = Physics2D.OverlapCircleAll(attackOrigin, radius, playerMask);

            foreach (var hit in hits)
            {
                Vector2 toTarget = ((Vector2)hit.transform.position - attackOrigin).normalized;
                float angle = Vector2.Angle(forward, toTarget);

                if (angle <= maxAngle)
                {
                    // 벽 체크
                    Collider2D playerCollider = hit;

                    Vector2[] checkPoints = new Vector2[]
                    {
                        playerCollider.bounds.center, // 중심
                        playerCollider.bounds.min, // 좌하단
                        playerCollider.bounds.max, // 우상단
                        new Vector2(playerCollider.bounds.min.x, playerCollider.bounds.max.y), // 좌상단
                        new Vector2(playerCollider.bounds.max.x, playerCollider.bounds.min.y) // 우하단
                    };

                    bool allBlocked = true;
                    foreach (var point in checkPoints)
                    {
                        RaycastHit2D wallCheck = Physics2D.Linecast(attackOrigin, point, obstacleMask);
                        if (wallCheck.collider == null)
                        {
                            allBlocked = false; // 한 곳이라도 뚫렸다면 피격
                            break;
                        }
                    }

                    if (allBlocked)
                    {
                        // 몸 전체가 벽 뒤에 있다면 피격받지 않음
                        continue; 
                    }

                    // 벽 없으면 대미지 부여
                    float damage = controller.Status.attack * damageMultiply;
                    Transform target = hit.transform;
                    controller.DealDamageToPlayer(damage, target, false);
                    break;
                }
            }

            float angle2 = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;
            Destroy(Instantiate(attackEffect, controller.transform.position, Quaternion.Euler(0, 0, angle2)), 1.0f);
        }
    }
}
