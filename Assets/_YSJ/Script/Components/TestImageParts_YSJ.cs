using UnityEngine;

public class TestImageParts_YSJ : MonoBehaviour, IImageParts_YSJ
{
    public void AfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
        print("쏜후");
    }

    public void BeforeFire(Artifact_YSJ fireArtifact)
    {
        print("쏘기전");
    }

    public void OnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
        print("맞음");
    }
}
