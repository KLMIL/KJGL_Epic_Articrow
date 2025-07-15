using UnityEngine;

/// <summary>
/// 이동 속도 증가 퍼센트의 100%만큼 모든 피해량 증가
/// </summary>
public class ImageParts_Passive_AttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ, IImagePartsToSkillAttack_YSJ
{
    float _increasePercent = 100f;

    public override string partsName => "Passive_AttackPower_1";

    #region [Normal]
    public void NormalAttackPessive(Artifact_YSJ fireArtifact)
    {
    }

    public void NormalAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
    }

    public void NormalAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
        AttackPower(fireArtifact, spawnedAttack, fireArtifact.normalStatus.Default_AttackPower);
    }

    public void NormalAttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void NormalAttackOnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
    }
    #endregion

    #region [Skill]
    public void SkillAttackPessive(Artifact_YSJ fireArtifact)
    {
    }

    public void SkillAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
    }

    public void SkillAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
        AttackPower(fireArtifact, spawnedAttack, fireArtifact.skillStatus.Default_AttackPower);
    }

    public void SkillAttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void SKillAttackOnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
    }
    #endregion

    #region [상세]
    void AttackPower(Artifact_YSJ fireArtifact, GameObject spawnedAttack, float default_AttackPower)
    {
        float addedPercent = fireArtifact.Added_MoveSpeed / fireArtifact.playerStatus.DefaultMoveSpeed;
        addedPercent = Mathf.RoundToInt(addedPercent * 100f) / 100f;
        Debug.Log($"[ckt] {partsName} addPercent_{addedPercent}");

        float add = _increasePercent * addedPercent * default_AttackPower;
        add = Mathf.RoundToInt(add) / 100f;
        //Debug.Log($"[ckt] {partsName} add_{add}");

        spawnedAttack.GetComponent<MagicRoot_YSJ>().AttackPower += add;
        //Debug.Log($"[ckt] {partsName} {addedPercent}_{add}");
    }
    #endregion
}
