using UnityEngine;

public class ImageParts_Passive_SkillAttackpower_1 : ImagePartsRoot_YSJ, IImagePartsToSkillAttack_YSJ
{
    public override string partsName => "Passive_SkillAttackpower_1";

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
    }

    public void SkillAttackPessive(Artifact_YSJ fireArtifact)
    {
        //스킬 시전 시간 증가
        fireArtifact.Added_SkillAttackStartDelay += 0.15f;
        
        //스킬 공격 피해 증가
        fireArtifact.Added_SkillAttackPower += 0.15f * fireArtifact.Default_NormalAttackPower;

        Debug.Log($"[ckt] {partsName} {fireArtifact.Added_SkillAttackStartDelay}_{fireArtifact.Added_SkillAttackPower}");
    }
}
