using UnityEngine;

public class ImageParts_Hit_Explosion_1 : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ, IImagePartsToSkillAttack_YSJ
{
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
        GameObject explosion = YSJ.Managers.TestPool.Get<GameObject>(Define.PoolID.CastExplosion);
        explosion.transform.position = hitObject.transform.position;
        explosion.GetComponent<CKT.Explosion>().Init(1);
        Debug.Log($"[ckt] {partsName} explosion");
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
        GameObject explosion = YSJ.Managers.TestPool.Get<GameObject>(Define.PoolID.CastExplosion);
        explosion.transform.position = hitObject.transform.position;
        explosion.GetComponent<CKT.Explosion>().Init(1);
        Debug.Log($"[ckt] {partsName} explosion");
    }

    public void SkillAttackPessive(Artifact_YSJ fireArtifact)
    {
    }
    #endregion
}
