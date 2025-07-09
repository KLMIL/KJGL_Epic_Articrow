using Game.Enemy;
using UnityEngine;

/// <summary>
/// 스킬로 대상을 처치했을 때 현재 공격의 250%에 해당하는 광역 공격 생성
/// </summary>
public class ImageParts_KillSkill_Explosion_1 : ImagePartsRoot_YSJ, IImagePartsToSkillAttack_YSJ
{
    float _damagePercent = 250f;

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
        EnemyController enemyController = hitObject.transform.root.GetComponentInChildren<EnemyController>();
        if (enemyController == null)
            return;

        float enemyHealth = enemyController.Status.healthPoint;
        float attackPower = spawnedAttack.GetComponent<MagicRoot_YSJ>().AttackPower;
        Debug.Log($"[ckt] {partsName} enemyHealth:{enemyHealth}, attackPower:{attackPower}");
        if (enemyHealth <= attackPower)
        {
            //폭발 생성
            GameObject explosion = YSJ.Managers.Pool.Get<GameObject>(Define.PoolID.HitExplosion);
            explosion.transform.position = hitObject.transform.position;

            float damage = _damagePercent * spawnedAttack.GetComponent<MagicRoot_YSJ>().AttackPower;
            damage = 0.01f * Mathf.RoundToInt(damage);

            explosion.GetComponent<CKT.Explosion>().Init(damage);
            Debug.Log($"[ckt] {partsName} KillExplosion {damage}");
        }
    }
    #endregion
}
