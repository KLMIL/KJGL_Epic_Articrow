using System.Collections;
using UnityEngine;

public class ImageParts_AfterSkill_NormalAttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToSkillAttack_YSJ
{
    public override string partsName => "AfterSkill_NormalAttackPower_1";

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

    #region [스킬 시전 시 5초 동안 일반 공격 피해량 증가 (현재 스킬에는 미적용)]
    void OnDisable()
    {
        StopBuff();
    }

    Artifact_YSJ _fireArtifact = null;
    Coroutine _normalAttackPowerCoroutine = null;
    float _duration = 5f;

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
        float add = 0.15f * _fireArtifact.normalStatus.Default_AttackPower;

        _fireArtifact.normalStatus.Added_AttackPower += add;
        Debug.Log($"[ckt] {partsName} StartBuff {add}");
    }

    void EndBuff()
    {
        float add = 0.15f * _fireArtifact.normalStatus.Default_AttackPower;

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
