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
        float addMoveSpeed = _increasePercent * playerStatus.DefaultMoveSpeed;
        addMoveSpeed = Mathf.RoundToInt(addMoveSpeed) / 100f;
        float addDashCoolTime = _increasePercent * playerStatus.DefaultDashCoolTime;
        addDashCoolTime = Mathf.RoundToInt(addDashCoolTime) / 100f;

        currentArtifact.Added_MoveSpeed += addMoveSpeed;
        currentArtifact.Added_DashCoolTime += addDashCoolTime;
        Debug.Log($"[ckt] {partsName} {addMoveSpeed}_{addDashCoolTime}");
    }
    #endregion
}
