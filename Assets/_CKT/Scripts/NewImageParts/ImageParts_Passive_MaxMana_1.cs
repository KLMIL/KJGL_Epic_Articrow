using UnityEngine;

/// <summary>
/// 최대 마나 1 증가
/// </summary>
public class ImageParts_Passive_MaxMana_1 : ImagePartsRoot_YSJ, IImagePartsToEnhance_YSJ
{
    public override string partsName => "Passive_MaxMana_1";

    #region [Equip]
    public void Equip(Artifact_YSJ currentArtifact)
    {
        int add = 1;
        
        currentArtifact.playerStatus.OffsetMaxMana += add;
        Debug.Log($"[ckt] {partsName} AddMaxMana({add})");
    }
    #endregion
}
