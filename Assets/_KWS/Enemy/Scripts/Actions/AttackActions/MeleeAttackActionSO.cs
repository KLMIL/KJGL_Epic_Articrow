using UnityEngine;
using UnityEngine.InputSystem;

/*
 * 물리 공격(몬스터가 직접 근접해서 공격하는) 정의
 * Contact: 몸통박치기
 * Basic: 일반 공격
 * Rush: 돌진 공격
 */
public enum MeleeAttackMode { Contact, Basic, Rush }
[CreateAssetMenu(
    fileName = "MeleeAttackAction",
    menuName = "Enemy/Action/Attack/Melee Attack"
)]
public class MeleeAttackActionSO : EnemyActionSO
{
    public MeleeAttackMode meleeAttackMode = MeleeAttackMode.Contact;

    [Header("Common Field: Contact Attack")]
    public float damageMultiply = 1.0f;
    public float knockbackMultiply = 1.0f;
    public float cooldown = 1.0f;


    [Header("Basic Attack Only")]
    public float attackRange = 1.0f;
    public Vector2 attackOffset = Vector2.right;

    [Header("Rush Attack Only")]
    public float rushSpeedMultiply = 6f;
    public bool trackingRush = false;

    public override void Act(EnemyController controller)
    {
        // 현재 공격 상태의 쿨타임 체크
        string key = controller.CurrentStateName;
        if (!controller.lastAttackTimes.ContainsKey(key))
        {
            controller.lastAttackTimes[key] = -Mathf.Infinity;
        }

        if (Time.time - controller.lastAttackTimes[key] < cooldown) return;


        // 공격 처리
        switch (meleeAttackMode)
        {
            case MeleeAttackMode.Contact:
                ContactAttack(controller);
                break;
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
        controller.lastAttackTimes[controller.CurrentStateName] = -Mathf.Infinity;
    }

    public override void OnExit(EnemyController controller)
    {
        switch (meleeAttackMode)
        {
            case MeleeAttackMode.Contact:
                break;
            case MeleeAttackMode.Basic:
                break;
            case MeleeAttackMode.Rush:
                controller.isRushing = false;
                break;
        }
    }



    private void ContactAttack(EnemyController controller)
    {
        float damage = controller.Status.attack * damageMultiply;
        controller.DealDamageToPlayer(damage, false);

        // (이펙트가 필요하다면 이 부분에 추가)
    }

    private void BasicAttack(EnemyController controller)
    {
        Vector2 attackOrigin = (Vector2)controller.transform.position + attackOffset;

        LayerMask playerMask = LayerMask.GetMask("Player");
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackOrigin, attackRange, playerMask);

        foreach (var hit in hits)
        {
            float damage = controller.Status.attack * damageMultiply;
            controller.DealDamageToPlayer(damage, false);

            // (넉백이 필요하다면 이 부분에 추가)
        }
        // (이팩트가 필요하다면 이 부분에 추가)
    }

    private void RushAttack(EnemyController controller)
    {
        if (trackingRush) // 유도형 돌진
        {
            // 매 프레임마다 최신 방향을 추적
            controller.rushDirection = (controller.Player.position - controller.transform.position).normalized;
            controller.isRushing = true;
        }
        else // 일반 돌진
        {
            // 첫 rush에서 방향 초기화
            if (!controller.isRushing)
            {
                controller.rushDirection = (controller.Player.position - controller.transform.position).normalized;
                controller.isRushing = true;
            }
        }

        controller.transform.Translate(controller.rushDirection * controller.Status.moveSpeed * rushSpeedMultiply * Time.deltaTime);
    }
}