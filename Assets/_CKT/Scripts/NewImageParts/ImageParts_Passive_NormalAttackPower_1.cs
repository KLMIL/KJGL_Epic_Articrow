using UnityEngine;

public class ImageParts_Passive_NormalAttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToEnhance_YSJ
{
    public override string partsName => "Passive_NormalAttackPower_1";

    public void Equip(Artifact_YSJ currentArtifact)
    {
        float add = 0.15f * currentArtifact.Default_NormalAttackPower;

        currentArtifact.Added_NormalAttackPower += add;
        Debug.Log($"[ckt] {partsName} {add}");
    }
}
