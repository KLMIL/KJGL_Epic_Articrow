using UnityEngine;

public interface IImagePartsToNormalAttack_YSJ
{
    public void NormalAttackPessive(Artifact_YSJ fireArtifact);
    public void NormalAttackBeforeFire(Artifact_YSJ fireArtifact);
    public void NormalAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack);
    public void NormalAttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack);
    public void NormalAttackOnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject);
}
