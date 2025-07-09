using System.Collections;
using UnityEngine;

/// <summary>
/// 스킬 적중 시 모든 공격 피해량 20% 증가 (3초 지속)
/// </summary>
public class ImageParts_HitSkill_AttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ, IImagePartsToSkillAttack_YSJ
{
    Coroutine _attackPowerCoroutine = null;
    float _increasePercent = 20f;
    float _duration = 3f;
    bool _isBuff = false;

    public override string partsName => "HitSkill_AttackPower_1";

    #region [Normal]
    public void NormalAttackPessive(Artifact_YSJ fireArtifact)
    {
        if (_isBuff)
        {
            float addNormal = _increasePercent * fireArtifact.normalStatus.Default_AttackPower;
            addNormal = 0.01f * Mathf.RoundToInt(addNormal);

            fireArtifact.normalStatus.Added_AttackPower += addNormal;
            Debug.Log($"[ckt] {partsName} NormalBuff {addNormal}");
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
        if (_isBuff)
        {
            float addSkill = (_increasePercent * 0.01f) * fireArtifact.skillStatus.Default_AttackPower;

            fireArtifact.skillStatus.Added_AttackPower += addSkill;
            Debug.Log($"[ckt] {partsName} SkillBuff {addSkill}");
        }
    }

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
        if (_attackPowerCoroutine != null)
        {
            EndBuff();
            StopBuff();
        }
        _attackPowerCoroutine = StartCoroutine(AttackPowerCoroutine(fireArtifact));
    }
    #endregion

    #region [상세]
    void OnDisable()
    {
        if (_attackPowerCoroutine != null)
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
        _attackPowerCoroutine = null;
    }

    void StartBuff()
    {
        _isBuff = true;
        Debug.Log($"[ckt] {partsName} Buff {_isBuff}");
    }

    void EndBuff()
    {
        _isBuff = false;
        Debug.Log($"[ckt] {partsName} Buff {_isBuff}");
    }

    void StopBuff()
    {
        if (_attackPowerCoroutine != null)
        {
            StopCoroutine(_attackPowerCoroutine);
            _attackPowerCoroutine = null;
        }
    }
    #endregion
}
