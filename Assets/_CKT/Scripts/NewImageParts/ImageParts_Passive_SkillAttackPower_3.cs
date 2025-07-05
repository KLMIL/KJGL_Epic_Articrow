using UnityEngine;

public class ImageParts_Passive_SkillAttackPower_3 : ImagePartsRoot_YSJ, IImagePartsToEnhance_YSJ
{
    public override string partsName => "Passive_SkillAttackPower_3";

    public void Equip(Artifact_YSJ currentArtifact)
    {
        float add = 25f * currentArtifact.playerStatus.OffsetMaxMana;
        add = Mathf.Clamp(add, 0, 75f);

        currentArtifact.skillStatus.Added_AttackPower += add;
        Debug.Log($"[ckt] {partsName} {add}");
    }
}
