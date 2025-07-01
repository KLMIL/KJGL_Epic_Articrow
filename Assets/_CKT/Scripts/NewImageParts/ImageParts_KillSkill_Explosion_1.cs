using CKT;
using Game.Enemy;
using System.Collections;
using UnityEngine;

public class ImageParts_KillSkill_Explosion_1 : ImagePartsRoot_YSJ, IImagePartsToSkillAttack_YSJ
{
    public override string partsName => "KillSkill_Explosion_1";

    public void SkillAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
        throw new System.NotImplementedException();
    }

    public void SkillAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
        throw new System.NotImplementedException();
    }

    public void SkillAttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
        throw new System.NotImplementedException();
    }

    public void SKillAttackOnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
        StartCoroutine(KillExplosionCoroutine(fireArtifact, spawnedAttack, hitObject));
    }

    public void SkillAttackPessive(Artifact_YSJ fireArtifact)
    {
        throw new System.NotImplementedException();
    }

    #region [스킬에 적중 된 대상이 처치되면 주변에 광역 피해]
    IEnumerator KillExplosionCoroutine(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
        yield return null;
        
        float enemyHealth = hitObject.GetComponent<EnemyController>().Status.healthPoint;
        if (enemyHealth <= 0)
        {
            //TODO : 폭발 생성
            GameObject hitExplosion = YSJ.Managers.TestPool.Get<GameObject>(Define.PoolID.HitExplosion);
            hitExplosion.transform.position = hitObject.transform.position;
            hitExplosion.GetComponent<Explosion>().Init(1);

            Debug.Log($"[ckt] {partsName} explosion");
        }
    }
    #endregion
}
