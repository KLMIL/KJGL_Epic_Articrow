using UnityEngine;

/// <summary>
/// 최대 마나 1 증가
/// </summary>
public class ImageParts_Passive_MaxMana_1 : ImagePartsRoot_YSJ, IImagePartsToEnhance_YSJ
{
    int _increaseValue = 2;

    public override string partsName => "Passive_MaxMana_1";

    #region [Equip]
    public void Equip(Artifact_YSJ currentArtifact)
    {
        int add = _increaseValue;
        
        currentArtifact.Added_MaxMana += add;
        //Debug.Log($"[ckt] {partsName} AddMaxMana({add})");
    }
    #endregion
}
