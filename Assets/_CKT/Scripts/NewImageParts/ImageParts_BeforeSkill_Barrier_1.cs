using System.Collections;
using UnityEngine;

/// <summary>
/// 스킬 시전 시 보호막 1 획득 (3초 지속)
/// </summary>
public class ImageParts_BeforeSkill_Barrier_1 : ImagePartsRoot_YSJ, IImagePartsToSkillAttack_YSJ
{
    Coroutine _barrierCoroutine = null;
    Artifact_YSJ _fireArtifact = null;
    int increaseValue = 1;
    float _duration = 3f;

    public override string partsName => "BeforeSkill_Barrier_1";

    #region [Skill]
    public void SkillAttackPessive(Artifact_YSJ fireArtifact)
    {
    }

    public void SkillAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
        if (_barrierCoroutine != null)
        {
            EndBuff();
            StopBuff();
        }
        _barrierCoroutine = StartCoroutine(BarrierCoroutine(fireArtifact));
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
    #endregion

    #region [상세]
    void OnDisable()
    {
        if (_barrierCoroutine != null)
        {
            EndBuff();
            StopBuff();
        }
    }

    IEnumerator BarrierCoroutine(Artifact_YSJ fireArtifact)
    {
        _fireArtifact = fireArtifact;
        StartBuff();

        yield return new WaitForSeconds(_duration);

        EndBuff();
        _fireArtifact = null;
        _barrierCoroutine = null;
    }

    void StartBuff()
    {
        int add = increaseValue;

        _fireArtifact.playerStatus.OffsetBarrier += add;
        Debug.Log($"[ckt] {partsName} StartBuff {add}");
    }

    void EndBuff()
    {
        int add = increaseValue;

        _fireArtifact.playerStatus.OffsetBarrier -= add;
        Debug.Log($"[ckt] {partsName} EndBuff {add}");
    }

    void StopBuff()
    {
        if (_barrierCoroutine != null)
        {
            StopCoroutine(_barrierCoroutine);
            _fireArtifact = null;
            _barrierCoroutine = null;
        }
    }
    #endregion
}