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

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, Speed, layerMask);

        if (hit)
        {
            Vector2 hitpoint = hit.point;
            lineRenderer.SetPosition(1, hitpoint);
            if (hit.collider.TryGetComponent<IDamagable>(out IDamagable damagable)) 
            {
                damagable.TakeDamage(AttackPower);
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
