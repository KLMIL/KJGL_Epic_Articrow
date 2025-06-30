using UnityEngine;

public class MagicRoot_YSJ : MonoBehaviour
{
    public Artifact_YSJ ownerArtifact;

    public float Speed;
    public float LifeTime;
    public float AttackPower;

    public enum MagicType
    {
        None,
        NormalAttack,
        SkillAttack
    }
    public MagicType magicType;

    public void BulletInitialize(Artifact_YSJ ownerArtifact) 
    {
        this.ownerArtifact = ownerArtifact;

        Speed = ownerArtifact.Current_NormalBulletSpeed;
        LifeTime = ownerArtifact.Current_NormalAttackLifeTime;
        AttackPower = ownerArtifact.Current_NormalAttackPower;
    }

    public void OnHit(Collider2D hitObject)
    {
        switch (magicType) 
        {
            case MagicType.None:
                print("마법타입이 뭔지 모름!");
                break;
            case MagicType.NormalAttack:
                ownerArtifact.HitNormalAttack?.Invoke(ownerArtifact, gameObject, hitObject.gameObject);
                break;
            case MagicType.SkillAttack:
                ownerArtifact.HitSkillAttack?.Invoke(ownerArtifact, gameObject, hitObject.gameObject);
                break;
        }
    }
}
