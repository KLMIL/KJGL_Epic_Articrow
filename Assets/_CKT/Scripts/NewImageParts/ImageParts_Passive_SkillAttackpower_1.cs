using UnityEngine;

public class ImageParts_Passive_SkillAttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToEnhance_YSJ
{
    public override string partsName => "Passive_SkillAttackPower_1";

    public void Equip(Artifact_YSJ currentArtifact)
    {
        float addDelay = 0.15f;
        float addPower = 0.15f * currentArtifact.normalStatus.Default_AttackPower;

        currentArtifact.skillStatus.Added_AttackStartDelay += addDelay;
        currentArtifact.skillStatus.Added_AttackPower += addPower;
        Debug.Log($"[ckt] {partsName} {addDelay}_{addPower}");
    }
}
