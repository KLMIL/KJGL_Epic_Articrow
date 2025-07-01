using UnityEngine;

public interface IImagePartsToSkillAttack_YSJ
{
    public void SkillAttackPessive(Artifact_YSJ fireArtifact);
    public void SkillAttackBeforeFire(Artifact_YSJ fireArtifact);
    public void SkillAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack);
    public void SkillAttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack);
    public void SKillAttackOnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject);
}
