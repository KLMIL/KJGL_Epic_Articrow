using System.Collections;
using UnityEngine;

/// <summary>
/// 스킬 시전 시 일반 공격 피해량 20% 증가 (5초 지속)
/// </summary>
public class ImageParts_AfterSkill_NormalAttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ, IImagePartsToSkillAttack_YSJ
{
    Coroutine _normalAttackPowerCoroutine = null;
    float _increasePercent = 20f;
    float _duration = 5f;
    bool _isBuff = false;

    public override string partsName => "AfterSkill_NormalAttackPower_1";

    #region [Normal]
    public void NormalAttackPessive(Artifact_YSJ fireArtifact)
    {
        if (_isBuff)
        {
            float add = _increasePercent * fireArtifact.normalStatus.Default_AttackPower;
            add = 0.01f * Mathf.RoundToInt(add);

            fireArtifact.normalStatus.Added_AttackPower += add;
            Debug.Log($"[ckt] {partsName} buff {add}");
        }
    }

    public void NormalAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
    }

    public void NormalAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
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
        if (_normalAttackPowerCoroutine != null)
        {
            StopBuff();
        }
        _normalAttackPowerCoroutine = StartCoroutine(AttackPowerCoroutine(fireArtifact));
    }

    public void SkillAttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void SKillAttackOnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
    }
    #endregion

    #region [상세]
    void OnDisable()
    {
        if (_normalAttackPowerCoroutine != null)
        {
            EndBuff();
            StopBuff();
        }
    }

    IEnumerator AttackPowerCoroutine(Artifact_YSJ fireArtifact)
    {
        StartBuff();

        yield return new WaitForSeconds(_duration);

        EndBuff();
        _normalAttackPowerCoroutine = null;
    }

    void StartBuff()
    {
        _isBuff = true;
        Debug.Log($"[ckt] {partsName} StartBuff {_isBuff}");
    }

    void EndBuff()
    {
        _isBuff = false;
        Debug.Log($"[ckt] {partsName} StartBuff {_isBuff}");
    }

    void StopBuff()
    {
        StopCoroutine(_normalAttackPowerCoroutine);
        _normalAttackPowerCoroutine = null;
    }
    #endregion
}
