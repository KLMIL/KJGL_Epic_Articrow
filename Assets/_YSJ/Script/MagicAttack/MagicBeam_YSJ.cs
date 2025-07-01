using UnityEngine;

public class MagicBeam_YSJ : MagicRoot_YSJ
{
    LineRenderer lineRenderer;
    LayerMask layerMask;
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
                if (i == DestroyCount) break;

                hitpoint = hits[i].point;
                if (hits[i].collider.TryGetComponent<IDamagable>(out IDamagable damagable))
                {
                    damagable.TakeDamage(AttackPower);
                }
            }

            if (hits.Length < DestroyCount) 
            {
                lineRenderer.SetPosition(1, transform.position + transform.right * Speed);
            }
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
}
