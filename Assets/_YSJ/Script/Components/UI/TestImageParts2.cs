using UnityEngine;

public class TestImageParts2 : ImagePartsRoot_YSJ, IImageParts_YSJ
{
    public override string partsName => "Test";

    public void AfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void AttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void OnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
    }

    public void Pessive(Artifact_YSJ fireArtifact)
    {
        print("발사 전 패시브2");
    }
}
