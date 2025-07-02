using System.Collections;
using UnityEngine;

public class ImageParts_BeforeSkill_Barrier_1 : ImagePartsRoot_YSJ, IImagePartsToSkillAttack_YSJ
{
    public override string partsName => "BeforeSkill_Barrier_1";

    public void SkillAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
        StopBuff();
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

    public void SkillAttackPessive(Artifact_YSJ fireArtifact)
    {
    }

    #region [스킬 공격 시전 시 3초간 보호막 획득]
    void OnDisable()
    {
        StopBuff();
    }

    Coroutine _barrierCoroutine = null;
    Artifact_YSJ _fireArtifact = null;
    float _duration = 3f;

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
        int add = 1;

        _fireArtifact.playerStatus.OffsetBarrier += add;
        Debug.Log($"[ckt] {partsName} StartBuff {add}");
    }

    void EndBuff()
    {
        int add = 1;

        _fireArtifact.playerStatus.OffsetBarrier -= add;
        Debug.Log($"[ckt] {partsName} StartBuff {add}");
    }

    void StopBuff()
    {
        if (_barrierCoroutine != null)
        {
            StopCoroutine(_barrierCoroutine);
            EndBuff();
            _fireArtifact = null;
            _barrierCoroutine = null;
        }
    }
    #endregion
}