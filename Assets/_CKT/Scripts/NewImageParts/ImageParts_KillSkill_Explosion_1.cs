using Game.Enemy;
using UnityEngine;

/// <summary>
/// 스킬로 대상을 처치했을 때 현재 공격의 150%에 해당하는 광역 공격 생성
/// </summary>
public class ImageParts_KillSkill_Explosion_1 : ImagePartsRoot_YSJ, IImagePartsToSkillAttack_YSJ
{
    float _damagePercent = 150f;

    public override string partsName => "KillSkill_Explosion_1";

    #region [Skill]
    public void SkillAttackPessive(Artifact_YSJ fireArtifact)
    {
    }

    public void SkillAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
    }

    public void SkillAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void SkillAttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void SKillAttackOnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
        EnemyController enemy = hitObject.transform.GetComponentInParent<EnemyController>();
        MagicRoot_YSJ magic = spawnedAttack.GetComponent<MagicRoot_YSJ>();

        if ((enemy != null) && (magic != null))
        {
            float enemyHealth = enemy.Status.healthPoint;
            float attackPower = magic.AttackPower;
            //Debug.Log($"[ckt] {partsName} enemyHealth:{enemyHealth}, attackPower:{attackPower}");
            if (enemyHealth <= attackPower)
            {
                //폭발 생성
                GameObject explosion = YSJ.Managers.Pool.Get<GameObject>(Define.PoolID.HitExplosion);
                explosion.transform.position = hitObject.transform.position;

                float damage = _damagePercent * spawnedAttack.GetComponent<MagicRoot_YSJ>().AttackPower;
                damage = Mathf.RoundToInt(damage) / 100f;

                explosion.GetComponent<CKT.Explosion>().Init(damage);
                Debug.Log($"[ckt] {partsName} KillExplosion {damage}");
            }
        }
        else
        {
            Debug.LogWarning($"[ckt] {partsName} EnemyController or MagicRoot_YSJ is null");
        }
    }
    #endregion
}
