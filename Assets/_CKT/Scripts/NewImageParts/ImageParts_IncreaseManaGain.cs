using UnityEngine;

public class ImageParts_IncreaseManaGain : ImagePartsRoot_YSJ, IImageParts_YSJ
{
    public override string partsName => "IncreaseManaGain";

    public void AfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {

    }

    public void AttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {

    }

    public void OnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {

    }

    public void Pessive(Artifact_YSJ fireArtifact)
    {
        //일반 공격 쿨타임 증가
        fireArtifact.Added_NormalAttackCoolTime += 0.1f * fireArtifact.Default_NormalAttackCoolTime;

        //TODO : ImageParts_IncreaseManaGain 마나 획득량 증가

        Debug.Log($"[ckt] ImageParts_IncreaseManaGain {fireArtifact.Added_NormalAttackCoolTime}_{""}");
    }
}
