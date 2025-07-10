using UnityEngine;

public class MagicNormalBeam_YSJ : MagicRoot_YSJ
{
    LineRenderer lineRenderer;
    LayerMask layerMask;

    public float LightLifeTime = 0.2f;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        layerMask = LayerMask.GetMask("EnemyHurtBox", "Obstacle", "BreakableObj");

        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
    }

    private void Start()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.right, base.Speed * base.LifeTime, layerMask);

        if (hits.Length != 0)
        {
            Vector2 hitpoint = Vector2.zero;

            // DestroyCount만큼 적 때리기
            for (int i = 0; i < hits.Length; i++)
            {
                hitpoint = hits[i].point;
                if (LayerMask.LayerToName(hits[i].collider.gameObject.layer) == "Obstacle")
                {
                    DestroyCount = -1;
                    break;
                }
                else if (hits[i].collider.TryGetComponent<IDamagable>(out IDamagable damagable))
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
                lineRenderer.SetPosition(1, transform.position + (transform.right * base.Speed * base.LifeTime));
            }
            // 관통 못했으면 마지막 충돌지점에 선 그리기
            else
            {
                lineRenderer.SetPosition(1, hitpoint);
            }
        }
        else 
        {
            lineRenderer.SetPosition(1, transform.position + (transform.right * base.Speed * base.LifeTime));
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

    /*public override void NormalAttackInitialize(Artifact_YSJ ownerArtifact)
    {
        float lifeTime = LifeTime;
        base.NormalAttackInitialize(ownerArtifact);
        LifeTime = lifeTime;
    }*/
}
