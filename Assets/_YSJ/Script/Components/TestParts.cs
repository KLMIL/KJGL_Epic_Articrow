using System.Collections;
using System.Drawing;
using UnityEngine;

public class TestParts : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ, IImagePartsToEnhance_YSJ
{
    public override string partsName => "TestParts";

    public void Equip(Artifact_YSJ currentArtifact)
    {
        currentArtifact.artifactStatus.Added_MaxMana += 1;
    }

    public void NormalAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
        //print("발사 후 액션");
        //spawnedAttack.transform.localRotation = Quaternion.Euler(0, 0, 0);
        if (spawnedAttack.TryGetComponent<SpriteRenderer>(out SpriteRenderer sprite))
        {
            sprite.color = UnityEngine.Color.blue;
        }
    }

    public void NormalAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
    }

    public void NormalAttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
        //print("공격이 날아가는 중 액션");
        spawnedAttack.transform.localScale += Vector3.one * 0.001f;
        spawnedAttack.GetComponent<MagicRoot_YSJ>().Speed += 0.1f;
    }

    public void NormalAttackOnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
        //print("공격이 적중한 액션");
    }

    public void NormalAttackPessive(Artifact_YSJ fireArtifact)
    {
        //print("발사 전 패시브 액션");
        fireArtifact.artifactStatus.Added_NormalAttackPower += 10f;
        fireArtifact.artifactStatus.Added_NormalAttackCount += 1;
        fireArtifact.artifactStatus.Added_NormalAttackSpreadAngle += 10f;
    }
}
