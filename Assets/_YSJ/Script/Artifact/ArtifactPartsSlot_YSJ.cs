using UnityEngine;

public class ArtifactPartsSlot_YSJ : MonoBehaviour
{
    void Awake()
    {
        Artifact_YSJ parentArtifact = GetComponentInParent<Artifact_YSJ>();
        if (parentArtifact)
        {
            parentArtifact.SlotTransform = transform;
        }
    }
}
