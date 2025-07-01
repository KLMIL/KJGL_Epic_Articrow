using UnityEngine;

public class ImageParts_Passive_MaxMana_1 : ImagePartsRoot_YSJ, IImagePartsToEnhance_YSJ
{
    public override string partsName => "Passive_MaxMana_1";

    public void Equip(Artifact_YSJ currentArtifact)
    {
        currentArtifact.Added_MaxMana += 1;
        Debug.Log($"[ckt] {partsName} {1}");
    }
}
