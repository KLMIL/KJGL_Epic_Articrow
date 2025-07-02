using UnityEngine;

public class ImageParts_Passive_MoveSpeed_1 : ImagePartsRoot_YSJ, IImagePartsToEnhance_YSJ
{
    public override string partsName => "Passive_MoveSpeed_1";

    public void Equip(Artifact_YSJ currentArtifact)
    {
        YSJ.PlayerStatus playerStatus = currentArtifact.playerStatus;
        float addMoveSpeed = 0.2f * playerStatus.DefaultMoveSpeed;
        float addDashCoolTime = 0.1f * playerStatus.DefaultDashCoolTime;

        currentArtifact.Added_MoveSpeed += addMoveSpeed;
        currentArtifact.Added_DeahCoolTime += addDashCoolTime;
    }
}
