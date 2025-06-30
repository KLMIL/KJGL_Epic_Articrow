using UnityEngine;

public class ArtifactFirePosition_YSJ : MonoBehaviour
{
    void Awake()
    {
        Artifact_YSJ parentArtifact = GetComponentInParent<Artifact_YSJ>();
        if (parentArtifact)
        {
            parentArtifact.firePosition = transform;
        }
    }
}
