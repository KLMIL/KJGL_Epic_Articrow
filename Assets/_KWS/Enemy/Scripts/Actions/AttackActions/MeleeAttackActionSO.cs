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
    public enum MeleeAttackMode { Basic, Rush }
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

        public override void Act(EnemyController controller)
        {
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
            }


            // 쿨타임 부여
            controller.lastAttackTimes[key] = Time.time;
        }

        public override void OnEnter(EnemyController controller)
        {
            if (meleeAttackMode == MeleeAttackMode.Rush)
            {
                controller.FSM.rushSpeedMultiply = this.rushSpeedMultiply;
                controller.FSM.currentActionDamageMultiply = this.damageMultiply;
            }

            //controller.lastAttackTimes[controller.CurrentStateName] = -Mathf.Infinity;
        }

        public override void OnExit(EnemyController controller)
        {
            switch (meleeAttackMode)
            {
                //case MeleeAttackMode.Contact:
                //    break;
                case MeleeAttackMode.Basic:
                    break;
                case MeleeAttackMode.Rush:
                    controller.FSM.isRushing = false;
                    controller.FSM.currentActionDamageMultiply = 1.0f;
                    break;
            }
        }



        // DEP :: DealDamage로 완전히 이동
        //private void ContactAttack(EnemyController controller)
        //{
        //    float damage = controller.Status.attack * damageMultiply;
        //    controller.DealDamageToPlayer(damage, false);

        //    // (이펙트가 필요하다면 이 부분에 추가)
        //}

        private void BasicAttack(EnemyController controller)
        {
            // TODO: 구조 다시 구현할 것. EnemyDealDamage에서 대미지 부여하는 방식으로.
            //Vector2 attackOrigin = (Vector2)controller.transform.position + attackOffset;

            //LayerMask playerMask = LayerMask.GetMask("Player");
            //Collider2D[] hits = Physics2D.OverlapCircleAll(attackOrigin, attackRange, playerMask);

            //foreach (var hit in hits)
            //{
            //    float damage = controller.Status.attack * damageMultiply;
            //    controller.DealDamageToPlayer(damage, false);

            //    // (넉백이 필요하다면 이 부분에 추가)
            //}
            // (이팩트가 필요하다면 이 부분에 추가)

            Vector2 attackOrigin = (Vector2)controller.transform.position + attackOffset;
            float radius = attackRange;
            LayerMask playerMask = LayerMask.GetMask("Player");

            Collider2D[] hits = Physics2D.OverlapCircleAll(attackOrigin, radius, playerMask);

            foreach (var hit in hits)
            {
                // 태그까지 검사 -> 안전망 역할
                if (hit.CompareTag("Player") && hit.isTrigger)
                {
                    float damage = controller.Status.attack * damageMultiply;
                    Transform target = hit.transform;
                    controller.DealDamageToPlayer(damage, target, false);
                    break;
                }
            }
        }

        private void RushAttack(EnemyController controller)
        {
            // 유도형 돌진은 일단 제외
            //if (trackingRush) // 유도형 돌진
            //{
            //    // 매 프레임마다 최신 방향을 추적
            //    controller.rushDirection = (controller.Player.position - controller.transform.position).normalized;
            //}
            //else if (!controller.isRushing)// 일반 돌진
            if (!controller.FSM.isRushing)// 일반 돌진
            {
                // 첫 rush에서 방향 초기화
                controller.FSM.rushDirection = (controller.Player.position - controller.transform.position).normalized;
            }

            controller.FSM.rushDamageMultuply = damageMultiply;
            controller.FSM.isRushAttacked = false;
            controller.FSM.isRushing = true;
            controller.MoveTo(controller.FSM.rushDirection, rushDuration, "RushAttack");

            //controller.transform.Translate(controller.rushDirection * controller.Status.moveSpeed * rushSpeedMultiply * Time.deltaTime);
        }
    }
}
