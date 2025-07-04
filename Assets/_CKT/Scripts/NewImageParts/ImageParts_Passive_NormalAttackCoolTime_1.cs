using UnityEngine;

public class ImageParts_Passive_NormalAttackCoolTime_1 : ImagePartsRoot_YSJ, IImagePartsToEnhance_YSJ
{
    public override string partsName => "Passive_NormalAttackCoolTime_1";

    public void Equip(Artifact_YSJ currentArtifact)
    {
        float add = 0.15f * currentArtifact.normalStatus.Default_AttackCoolTime;

        currentArtifact.normalStatus.Added_AttackCoolTime -= add;
        Debug.Log($"[ckt] {partsName} {add}");
    }
}
