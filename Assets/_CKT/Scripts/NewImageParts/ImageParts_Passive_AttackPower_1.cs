using UnityEngine;

public class ImageParts_Passive_AttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToEnhance_YSJ
{
    public override string partsName => "Passive_AttackPower_1";

    public void Equip(Artifact_YSJ currentArtifact)
    {
        YSJ.PlayerStatus playerStatus = currentArtifact.playerStatus;
        float add = 0.01f * playerStatus.OffsetMoveSpeed;

        currentArtifact.artifactStatus.Added_NormalAttackPower += add;
        currentArtifact.artifactStatus.Added_SkillAttackPower += add;
        Debug.Log($"[ckt] {partsName} {add}");
    }
}
