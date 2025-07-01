using UnityEngine;

public class ImageParts_Passive_ManaGain_1 : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ
{
    public override string partsName => "Passive_ManaGain_1";

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
        //일반 공격 쿨타임 증가
        fireArtifact.Added_NormalAttackCoolTime += 0.1f * fireArtifact.Default_NormalAttackCoolTime;

        //TODO : ImageParts_IncreaseManaGain 마나 획득량 증가


        Debug.Log($"[ckt] Passive_ManaGain_1 {fireArtifact.Added_NormalAttackCoolTime}_{""}");
    }
}
