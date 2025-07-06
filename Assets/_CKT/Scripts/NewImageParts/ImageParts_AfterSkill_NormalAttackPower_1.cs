using System.Collections;
using UnityEngine;

/// <summary>
/// 스킬 시전 시 일반 공격 피해량 15% 증가
/// </summary>
public class ImageParts_AfterSkill_NormalAttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToSkillAttack_YSJ
{
    Artifact_YSJ _fireArtifact = null;
    Coroutine _normalAttackPowerCoroutine = null;
    float _increasePercent = 15f;
    float _duration = 5f;

    public override string partsName => "AfterSkill_NormalAttackPower_1";

    #region [Skill]
    public void SkillAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
    }

    public void SkillAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
        StopBuff();
        _normalAttackPowerCoroutine = StartCoroutine(AttackPowerCoroutine(fireArtifact));
    }

    public void SkillAttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void SKillAttackOnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
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

        _normalAttackPowerCoroutine = null;
    }

    void StartBuff()
    {
        float add = (_increasePercent * 0.01f) * _fireArtifact.normalStatus.Default_AttackPower;

        _fireArtifact.normalStatus.Added_AttackPower += add;
        Debug.Log($"[ckt] {partsName} StartBuff {add}");
    }

    void EndBuff()
    {
        float add = (_increasePercent * 0.01f) * _fireArtifact.normalStatus.Default_AttackPower;

        _fireArtifact.normalStatus.Added_AttackPower -= add;
        Debug.Log($"[ckt] {partsName} EndBuff {add}");
    }

    void StopBuff()
    {
        if (_normalAttackPowerCoroutine != null)
        {
            StopCoroutine(_normalAttackPowerCoroutine);
            EndBuff();
            _fireArtifact = null;
            _normalAttackPowerCoroutine = null;
        }
    }
    #endregion
}
