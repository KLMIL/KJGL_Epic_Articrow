using UnityEngine;

public class TestImageParts2 : ImagePartsRoot_YSJ, IImagePartsToSkillAttack_YSJ
{
    public override string partsName => "Test";

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
        print("스킬공격이 적중한 액션2");
    }

    public void SkillAttackPessive(Artifact_YSJ fireArtifact)
    {
    }
}
