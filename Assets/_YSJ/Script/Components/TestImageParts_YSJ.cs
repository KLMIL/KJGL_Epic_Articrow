using UnityEngine;

public class TestImageParts_YSJ : MonoBehaviour, IImagePartsToNormalAttack_YSJ
{
    public void NormalAttackPessive(Artifact_YSJ fireArtifact)
    {
        print("발사 전 패시브");
    }
    public void NormalAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
        print("쏜후");
    }

    public void NormalAttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
        print("날아가는중");
    }

    public void NormalAttackOnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
        print("맞음");
    }
}
