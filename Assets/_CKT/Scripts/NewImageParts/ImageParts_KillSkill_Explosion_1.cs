using Game.Enemy;
using System.Collections;
using UnityEngine;

/// <summary>
/// 스킬로 대상을 처치했을 때 현재 공격의 50%에 해당하는 광역 공격 생성
/// </summary>
public class ImageParts_KillSkill_Explosion_1 : ImagePartsRoot_YSJ, IImagePartsToSkillAttack_YSJ
{
    float _damagePercent = 50f;

    public override string partsName => "KillSkill_Explosion_1";

    #region [Skill]
    public void SkillAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void SkillAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
    }

    public void SkillAttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void SKillAttackOnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
        StartCoroutine(KillExplosionCoroutine(fireArtifact, spawnedAttack, hitObject));
    }

    public void SkillAttackPessive(Artifact_YSJ fireArtifact)
    {
    }
    #endregion

    #region [상세]
    IEnumerator KillExplosionCoroutine(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
        yield return null;
        
        float enemyHealth = hitObject.GetComponent<EnemyController>().Status.healthPoint;
        if (enemyHealth <= 0)
        {
            //폭발 생성
            GameObject explosion = YSJ.Managers.Pool.Get<GameObject>(Define.PoolID.HitExplosion);
            explosion.transform.position = hitObject.transform.position;

            float damage = (_damagePercent * 0.01f) * spawnedAttack.GetComponent<MagicRoot_YSJ>().AttackPower;
            explosion.GetComponent<CKT.Explosion>().Init(damage);
            Debug.Log($"[ckt] {partsName} KillExplosion {damage}");
        }
    }
    #endregion
}
