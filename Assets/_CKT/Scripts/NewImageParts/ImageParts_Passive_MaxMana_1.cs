using UnityEngine;

public class ImageParts_Passive_MaxMana_1 : ImagePartsRoot_YSJ, IImagePartsToEnhance_YSJ
{
    public override string partsName => "Passive_MaxMana_1";

    public void Equip(Artifact_YSJ currentArtifact)
    {
        int add = 1;
        
        currentArtifact.artifactStatus.Added_MaxMana += add;
        Debug.Log($"[ckt] {partsName} AddMaxMana({add})");
    }
}
