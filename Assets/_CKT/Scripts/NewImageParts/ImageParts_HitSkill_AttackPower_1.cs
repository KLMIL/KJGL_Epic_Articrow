using System.Collections;
using UnityEngine;

/// <summary>
/// 스킬 적중 시 모든 공격 피해량 15% 증가 (5초 지속)
/// </summary>
public class ImageParts_HitSkill_AttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToSkillAttack_YSJ
{
    Coroutine _attackPowerCoroutine = null;
    Artifact_YSJ _fireArtifact;
    float _increasePercent = 15f;
    float _duration = 5f;

    public override string partsName => "HitSkill_AttackPower_1";

    #region [Skill]
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
        StopBuff();
        _attackPowerCoroutine = StartCoroutine(AttackPowerCoroutine(fireArtifact));
    }

    public void SkillAttackPessive(Artifact_YSJ fireArtifact)
    {
    }
    #endregion

    #region [상세]
    void OnDisable()
    {
        StopBuff();
    }

    IEnumerator AttackPowerCoroutine(Artifact_YSJ fireArtifact)
    {
        _fireArtifact = fireArtifact;
        StartBuff();

        yield return new WaitForSeconds(_duration);

        EndBuff();
        _fireArtifact = null;
        _attackPowerCoroutine = null;
    }

    void StartBuff()
    {
        float addNormal = (_increasePercent * 0.01f) * _fireArtifact.normalStatus.Default_AttackPower;
        float addSkill = (_increasePercent * 0.01f) * _fireArtifact.skillStatus.Default_AttackPower;
        
        _fireArtifact.normalStatus.Added_AttackPower += addNormal;
        _fireArtifact.skillStatus.Added_AttackPower += addSkill;
        Debug.Log($"[ckt] {partsName} StartBuff {addNormal}_{addSkill}");
    }

    void EndBuff()
    {
        float addNormal = (_increasePercent * 0.01f) * _fireArtifact.normalStatus.Default_AttackPower;
        float addSkill = (_increasePercent * 0.01f) * _fireArtifact.skillStatus.Default_AttackPower;
        
        _fireArtifact.normalStatus.Added_AttackPower -= addNormal;
        _fireArtifact.skillStatus.Added_AttackPower -= addSkill;
        Debug.Log($"[ckt] {partsName} EndBuff {addNormal}_{addSkill}");
    }

    void StopBuff()
    {
        if (_attackPowerCoroutine != null)
        {
            StopCoroutine(_attackPowerCoroutine);
            EndBuff();
            _fireArtifact = null;
            _attackPowerCoroutine = null;
        }
    }
    #endregion
}
