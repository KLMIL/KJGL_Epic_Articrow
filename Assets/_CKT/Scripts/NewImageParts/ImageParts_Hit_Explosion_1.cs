using UnityEngine;

/// <summary>
/// 공격 적중 시 현재 공격 피해량의 10%에 해당하는 광역 공격 생성
/// </summary>
public class ImageParts_Hit_Explosion_1 : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ, IImagePartsToSkillAttack_YSJ
{
    float _damagePercent = 10f;

    public override string partsName => "Hit_Explosion_1";

    #region [Normal]
    public void NormalAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void NormalAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
    }

    public void NormalAttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void NormalAttackOnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
        //폭발 생성
        GameObject explosion = YSJ.Managers.Pool.Get<GameObject>(Define.PoolID.CastExplosion);
        explosion.transform.position = hitObject.transform.position;

        float damage = (_damagePercent * 0.01f) * spawnedAttack.GetComponent<MagicRoot_YSJ>().AttackPower;
        explosion.GetComponent<CKT.Explosion>().Init(damage);
        Debug.Log($"[ckt] {partsName} HitExplosion {damage}");
    }

    public void NormalAttackPessive(Artifact_YSJ fireArtifact)
    {
    }
    #endregion

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
        //폭발 생성
        GameObject explosion = YSJ.Managers.Pool.Get<GameObject>(Define.PoolID.CastExplosion);
        explosion.transform.position = hitObject.transform.position;

        float damage = (_damagePercent * 0.01f) * spawnedAttack.GetComponent<MagicRoot_YSJ>().AttackPower;
        explosion.GetComponent<CKT.Explosion>().Init(damage);
        Debug.Log($"[ckt] {partsName} HitExplosion {damage}");
    }

    public void SkillAttackPessive(Artifact_YSJ fireArtifact)
    {
    }
    #endregion
}
