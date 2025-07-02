using System;
using UnityEngine;

public class MagicRoot_YSJ : MonoBehaviour
{
    public Artifact_YSJ ownerArtifact;

    public float Speed;
    public float LifeTime;
    protected float elapsedTime;
    public float AttackPower;
    public int DestroyCount = 0;

    public Action<Artifact_YSJ, GameObject> FlyingAction;
    public Action<Artifact_YSJ, GameObject, GameObject> OnHitAction;

    public virtual void NormalAttackInitialize(Artifact_YSJ ownerArtifact)
    {
        this.ownerArtifact = ownerArtifact;

        Speed = ownerArtifact.Current_NormalBulletSpeed;
        LifeTime = ownerArtifact.Current_NormalAttackLifeTime;
        AttackPower = ownerArtifact.Current_NormalAttackPower;

        FlyingAction += CountLifeTime;
    }

    public virtual void SkillAttackInitialize(Artifact_YSJ ownerArtifact) 
    {
        this.ownerArtifact = ownerArtifact;

        Speed = ownerArtifact.Current_SkillBulletSpeed;
        LifeTime = ownerArtifact.Current_SkillAttackLifeTime;
        AttackPower = ownerArtifact.Current_SkillAttackPower;

        FlyingAction += CountLifeTime;
    }

    public void OnHit(Collider2D hitObject)
    {
        OnHitAction?.Invoke(ownerArtifact, gameObject, hitObject.gameObject);
        DestroyCount--;
    }

    public virtual void CountLifeTime(Artifact_YSJ ownerArtifact, GameObject Attack) 
    {
        if (elapsedTime < LifeTime)
        {
            elapsedTime += Time.deltaTime;
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    public void CheckDestroy() 
    {
        if (DestroyCount < 0)
        {
            Destroy(gameObject);
        }
    }
}
