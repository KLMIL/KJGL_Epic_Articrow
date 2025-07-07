using UnityEngine;

/// <summary>
/// 이동 속도 20% 증가, 대시 쿨타임 20% 증가
/// </summary>
public class ImageParts_Passive_MoveSpeed_1 : ImagePartsRoot_YSJ, IImagePartsToEnhance_YSJ
{
    float _increasePercent = 20f;

    public override string partsName => "Passive_MoveSpeed_1";

    #region [Equip]
    public void Equip(Artifact_YSJ currentArtifact)
    {
        YSJ.PlayerStatus playerStatus = currentArtifact.playerStatus;
        float addMoveSpeed = (_increasePercent * 0.01f) * playerStatus.DefaultMoveSpeed;
        float addDashCoolTime = (_increasePercent * 0.01f) * playerStatus.DefaultDashCoolTime;

        currentArtifact.Added_MoveSpeed += addMoveSpeed;
        currentArtifact.Added_DeahCoolTime += addDashCoolTime;
        Debug.Log($"[ckt] {partsName} {addMoveSpeed}_{addDashCoolTime}");
    }
    #endregion
}
