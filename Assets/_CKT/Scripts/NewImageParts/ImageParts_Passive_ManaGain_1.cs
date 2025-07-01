using UnityEngine;

public class ImageParts_Passive_ManaGain_1 : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ
{
    public override string partsName => "Passive_ManaGain_1";

    public void NormalAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
    }

    public void NormalAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void NormalAttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void NormalAttackOnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
    }

    public void NormalAttackPessive(Artifact_YSJ fireArtifact)
    {
        float addNormal = 0.1f * fireArtifact.Default_NormalAttackCoolTime;

        fireArtifact.Added_NormalAttackCoolTime += addNormal;
        //TODO : ImageParts_IncreaseManaGain 마나 획득량 증가
        Debug.Log($"[ckt] {partsName} {fireArtifact.Added_NormalAttackCoolTime}_{""}");
    }
}
