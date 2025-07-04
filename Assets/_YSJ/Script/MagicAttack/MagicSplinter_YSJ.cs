using UnityEngine;

public class MagicSplinter_YSJ : MagicRoot_YSJ
{
    Rigidbody2D rb2d;
    Collider2D ignoreCollider;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        FlyingAction?.Invoke(ownerArtifact, gameObject);

        rb2d.linearVelocity = transform.right * Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IDamagable>() != null && collision != ignoreCollider)
        {
            OnHit(collision);
            collision.GetComponent<IDamagable>().TakeDamage(AttackPower);
            CheckDestroy();
        }
    }

    public void SplinterInitialize(MagicRoot_YSJ magicRoot, Collider2D ignoreCollision)
    {
        ownerArtifact = magicRoot.ownerArtifact;
        ignoreCollider = ignoreCollision;

        // 파츠슬롯 한바퀴 돌면서 탄에다가 직접등록
        for (int partsIndex = 0; partsIndex < ownerArtifact.MaxSlotCount; partsIndex++)
        {
            IImagePartsToSkillAttack_YSJ imageParts = ownerArtifact.SlotTransform.GetChild(partsIndex).GetComponentInChildren<IImagePartsToSkillAttack_YSJ>();
            if (imageParts != null)
            {
                FlyingAction += imageParts.SkillAttackFlying;
                OnHitAction += imageParts.SKillAttackOnHit;
            }
        }

        FlyingAction += CountLifeTime;
    }
}
