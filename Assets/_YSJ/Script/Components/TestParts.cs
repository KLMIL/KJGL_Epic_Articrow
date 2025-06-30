using UnityEngine;

public class TestParts : ImagePartsRoot_YSJ, IImageParts_YSJ
{
    public override string partsName => "TestParts";

    public void AfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
        //print("발사 후 액션");
        //spawnedAttack.transform.localRotation = Quaternion.Euler(0, 0, 0);
        spawnedAttack.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
    }

    public void AttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
        //print("공격이 날아가는 중 액션");
        spawnedAttack.transform.localScale += Vector3.one * 0.001f;
        spawnedAttack.GetComponent<MagicRoot_YSJ>().Speed += 0.1f;
    }

    public void OnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
        //print("공격이 적중한 액션");
    }

    public void Pessive(Artifact_YSJ fireArtifact)
    {
        //print("발사 전 패시브 액션");
        fireArtifact.Added_NormalAttackPower += 10f;
        fireArtifact.Added_NormalAttackCount += 1f;
        fireArtifact.Added_NormalAttackSpreadAngle += 10f;
    }
}
