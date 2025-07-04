using UnityEngine;

public class MagicSkillBeam_YSJ : MagicRoot_YSJ
{
    LineRenderer lineRenderer;
    LayerMask layerMask;

    public float LightLifeTime = 0.2f;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        layerMask = LayerMask.GetMask("EnemyHurtBox", "Obstacle");

        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
    }

    private void Start()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.right, Speed, layerMask);

        if (hits.Length != 0)
        {
            Vector2 hitpoint = Vector2.zero;

            // DestroyCount만큼 적 때리기
            for (int i = 0; i < hits.Length; i++)
            {
                hitpoint = hits[i].point;
                if (hits[i].collider.TryGetComponent<IDamagable>(out IDamagable damagable))
                {
                    OnHit(hits[i].collider);
                    damagable.TakeDamage(AttackPower);
                }
                if (DestroyCount < 0)
                {
                    break;
                }
            }
            // 관통 조건
            if (DestroyCount >= 0)
            {
                lineRenderer.SetPosition(1, transform.position + transform.right * Speed);
            }
            // 관통 못했으면 마지막 충돌지점에 선 그리기
            else
            {
                lineRenderer.SetPosition(1, hitpoint);
            }
        }
        else
        {
            lineRenderer.SetPosition(1, transform.position + transform.right * Speed);
        }
    }

    private void Update()
    {
        FlyingAction?.Invoke(ownerArtifact, gameObject);

        Color startColor = lineRenderer.startColor;

        startColor.a = Mathf.Lerp(1, 0, elapsedTime / LifeTime);

        lineRenderer.startColor = startColor;
        lineRenderer.endColor = startColor;

        lineRenderer.startWidth = Mathf.Lerp(transform.localScale.x, 0f, elapsedTime / LifeTime);
        lineRenderer.endWidth = Mathf.Lerp(transform.localScale.x, 0f, elapsedTime / LifeTime);
    }

    public override void SkillAttackInitialize(Artifact_YSJ ownerArtifact)
    {
        base.SkillAttackInitialize(ownerArtifact);

        LifeTime = LightLifeTime;

        // 파츠슬롯 한바퀴 돌면서 탄에다가 직접등록
        for (int partsIndex = 0; partsIndex < ownerArtifact.artifactStatus.MaxSlotCount; partsIndex++)
        {
            IImagePartsToSkillAttack_YSJ imageParts = ownerArtifact.artifactStatus.SlotTransform.GetChild(partsIndex).GetComponentInChildren<IImagePartsToSkillAttack_YSJ>();
            if (imageParts != null)
            {
                FlyingAction += imageParts.SkillAttackFlying;
                OnHitAction += imageParts.SKillAttackOnHit;
            }
        }
    }
}
