using UnityEngine;

public class ImageParts_Passive_NormalAttackCoolTime_1 : ImagePartsRoot_YSJ, IImagePartsToEnhance_YSJ
{
    public override string partsName => "Passive_NormalAttackCoolTime_1";

    public void Equip(Artifact_YSJ currentArtifact)
    {
        float add = 0.15f * currentArtifact.Default_NormalAttackCoolTime;

        currentArtifact.Added_NormalAttackCoolTime -= add;
        Debug.Log($"[ckt] {partsName} {add}");
    }
}
