using System.Collections;
using UnityEngine;

/// <summary>
/// 스킬 적중 시 이동 속도 20% 증가 (5초 지속)
/// </summary>
public class ImageParts_HitSkill_MoveSpeed_1 : ImagePartsRoot_YSJ, IImagePartsToSkillAttack_YSJ
{
    Artifact_YSJ _fireArtifact = null;
    Coroutine _moveSpeedUpCoroutine = null;
    float _increasePercent = 20f;
    float _duration = 5f;

    public override string partsName => "HitSkill_MoveSpeed_1";

    #region [Skill]
    public void SkillAttackPessive(Artifact_YSJ fireArtifact)
    {
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
        if (_moveSpeedUpCoroutine != null)
        {
            EndBuff();
            StopBuff();
        }
        _moveSpeedUpCoroutine = StartCoroutine(MoveSpeedUpCoroutine(fireArtifact));
    }
    #endregion

    #region [상세]
    void OnDisable()
    {
        if (_moveSpeedUpCoroutine != null)
        {
            EndBuff();
            StopBuff();
        }
    }

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
        float add = _increasePercent * playerStatus.DefaultMoveSpeed;
        add = 0.01f * Mathf.RoundToInt(add);

        _fireArtifact.Added_MoveSpeed += add;
        Debug.Log($"[ckt] {partsName} StartBuff {add}_{playerStatus.MoveSpeed}");
    }

    void EndBuff()
    {
        YSJ.PlayerStatus playerStatus = _fireArtifact.playerStatus;
        float add = _increasePercent * playerStatus.DefaultMoveSpeed;
        add = 0.01f * Mathf.RoundToInt(add);

        _fireArtifact.Added_MoveSpeed -= add;
        Debug.Log($"[ckt] {partsName} EndBuff {add}_{playerStatus.MoveSpeed}");
    }

    void StopBuff()
    {
        if (_moveSpeedUpCoroutine != null)
        {
            StopCoroutine(_moveSpeedUpCoroutine);
            _fireArtifact = null;
            _moveSpeedUpCoroutine = null;
        }
    }
    #endregion
}
