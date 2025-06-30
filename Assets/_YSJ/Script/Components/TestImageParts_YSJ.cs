using UnityEngine;

public class TestImageParts_YSJ : MonoBehaviour, IImageParts_YSJ
{
    public void Pessive(Artifact_YSJ fireArtifact)
    {
        print("발사 전 패시브");
    }
    public void AfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
        print("쏜후");
    }

    public void AttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
        print("날아가는중");
    }

    public void OnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
        print("맞음");
    }
}
