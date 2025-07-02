using UnityEngine;

public class ImageParts_Passive_ManaGain_1 : ImagePartsRoot_YSJ, IImagePartsToEnhance_YSJ
{
    public override string partsName => "Passive_ManaGain_1";

    public void Equip(Artifact_YSJ currentArtifact)
    {
        float addNormal = 0.1f * currentArtifact.Default_NormalAttackCoolTime;

        currentArtifact.Added_NormalAttackCoolTime += addNormal;
        //TODO : ImageParts_IncreaseManaGain 마나 획득량 증가
        Debug.Log($"[ckt] {partsName} {currentArtifact.Added_NormalAttackCoolTime}_{""}");
    }
}
