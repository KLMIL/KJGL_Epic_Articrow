using UnityEngine;

public interface IImageParts_YSJ
{
    public void BeforeFire(Artifact_YSJ fireArtifact);
    public void AfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack);
    public void OnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject);
}
