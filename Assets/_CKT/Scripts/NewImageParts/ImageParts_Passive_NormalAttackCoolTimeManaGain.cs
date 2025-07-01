using UnityEngine;

public class ImageParts_Passive_AttackCoolTimeManaGain : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ
{
    public override string partsName => "Passive_AttackCoolTimeManaGain";

    public void NormalAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
        throw new System.NotImplementedException();
    }

    public void NormalAttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
        throw new System.NotImplementedException();
    }

    public void NormalAttackOnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
        throw new System.NotImplementedException();
    }

    public void NormalAttackPessive(Artifact_YSJ fireArtifact)
    {
        //일반 공격 쿨타임 증가
        fireArtifact.Added_NormalAttackCoolTime += 0.1f * fireArtifact.Default_NormalAttackCoolTime;

        //TODO : ImageParts_IncreaseManaGain 마나 획득량 증가


        Debug.Log($"[ckt] Passive_AttackCoolTimeManaGain {fireArtifact.Added_NormalAttackCoolTime}_{""}");
    }
}
