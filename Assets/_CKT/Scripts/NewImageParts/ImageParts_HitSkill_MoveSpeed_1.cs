using System.Collections;
using UnityEngine;


public class ImageParts_HitSkill_MoveSpeed_1 : ImagePartsRoot_YSJ, IImagePartsToSkillAttack_YSJ
{
    public override string partsName => "HitSkill_MoveSpeed_1";

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
        _moveSpeedUpCoroutine = StartCoroutine(MoveSpeedUpCoroutine(fireArtifact));
    }

    public void SkillAttackPessive(Artifact_YSJ fireArtifact)
    {
    }

    #region [스킬 공격 적중 시 5초간 이동 속도 15% 증가]
    void OnDisable()
    {
        StopBuff();
    }

    Artifact_YSJ _fireArtifact = null;
    Coroutine _moveSpeedUpCoroutine = null;
    float _duration = 5f;

    IEnumerator MoveSpeedUpCoroutine(Artifact_YSJ fireArtifact)
    {
        _fireArtifact = fireArtifact;
        StartBuff();

        yield return new WaitForSeconds(_duration);

        EndBuff();
        _fireArtifact = null;

        _moveSpeedUpCoroutine = null;
    }

    void StartBuff()
    {
        YSJ.PlayerStatus playerStatus = _fireArtifact.playerStatus;
        float add = 0.15f * playerStatus.MoveSpeed;

        playerStatus.OffsetMoveSpeed += add;
        Debug.Log($"[ckt] {partsName} StartBuff {add}");
    }

    void EndBuff()
    {
        YSJ.PlayerStatus playerStatus = _fireArtifact.playerStatus;
        float add = 0.15f * playerStatus.MoveSpeed;

        playerStatus.OffsetMoveSpeed -= add;
        Debug.Log($"[ckt] {partsName} EndBuff {add}");
    }

    void StopBuff()
    {
        if (_moveSpeedUpCoroutine != null)
        {
            StopCoroutine(_moveSpeedUpCoroutine);
            EndBuff();
            _fireArtifact = null;
            _moveSpeedUpCoroutine = null;
        }
    }
    #endregion
}
