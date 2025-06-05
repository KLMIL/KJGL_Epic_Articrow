using UnityEngine;

/*
 * 물리 공격(몬스터가 직접 근접해서 공격하는) 정의
 * Contact: 몸통박치기
 * Basic: 일반 공격
 * Rush: 돌진 공격
 */
public enum AttackMode { Contact, Basic, Rush }
[CreateAssetMenu(
    fileName = "MeleeAttackAction",
    menuName = "Enemy/Action/Attack/Melee Attack"
)]
public class MeleeAttackActionSO : EnemyActionSO
{
    public AttackMode attackMode = AttackMode.Contact;

    [Header("Common Field: Contact Attack")]
    public float damageMultiply = 1.0f;
    public float knockbackMultiply = 1.0f;

    [Header("Basic Attack Only")]
    public bool TEMP2;

    [Header("Rush Attack Only")]
    public bool TEMP3;

    public override void Act(EnemyController controller)
    {
        switch (attackMode)
        {
            case AttackMode.Contact:
                ContactAttack(controller);
                break;
            case AttackMode.Basic:
                BasicAttack(controller);
                break;
            case AttackMode.Rush:
                RushAttack(controller);
                break;
        }
    }


    private void ContactAttack(EnemyController controller)
    {
        // TODO: 플레이어와 충돌 시 데미지/넉백 적용
    }

    private void BasicAttack(EnemyController controller)
    {
        // TODO: 일반 근접 공격 대미지/넉백 적용 등
    }

    private void RushAttack(EnemyController controller)
    {
        // TODO: 돌진 공격 대미지/넉백 적용 등
    }
}