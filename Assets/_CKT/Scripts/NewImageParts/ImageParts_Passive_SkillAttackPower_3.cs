using UnityEngine;

public class ImageParts_Passive_SkillAttackPower_3 : ImagePartsRoot_YSJ, IImagePartsToSkillAttack_YSJ
{
    public override string partsName => "Passive_SkillAttackPower_3";

    public void SkillAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void SkillAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
    }

    public void SkillAttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void SKillAttackOnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
    }

    public void SkillAttackPessive(Artifact_YSJ fireArtifact)
    {
        YSJ.PlayerStatus playerStatus = BMC.PlayerManager.Instance.PlayerStatus;
        float add = 25f * playerStatus.OffsetMaxMana;
        add = Mathf.Clamp(add, 0, 75f);

        fireArtifact.Added_SkillAttackPower += add;
        Debug.Log($"[ckt] {partsName} {add}");
    }
}
