using System.Collections;
using UnityEngine;

public class ImageParts_Passive_SkillAttackPower_2 : ImagePartsRoot_YSJ, IImagePartsToSkillAttack_YSJ
{
    public override string partsName => "Passive_SkillAttackPower_2";

    public void SkillAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
    }

    public void SkillAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
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
        StartCoroutine(SkillAttackPowerCoroutine(fireArtifact));
    }

    #region [시전 시간 비례 -> 스킬 피해 증가]
    IEnumerator SkillAttackPowerCoroutine(Artifact_YSJ fireArtifact)
    {
        yield return null;

        //최종 스킬 시전 시간의 30%만큼 스킬 공격 피해 증가
        fireArtifact.Added_SkillAttackPower += 0.3f * fireArtifact.Current_SkillAttackStartDelay;
        Debug.Log($"[ckt] {partsName} {fireArtifact.Added_SkillAttackPower}");
    }
    #endregion
}
