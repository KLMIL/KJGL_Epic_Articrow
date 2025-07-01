using UnityEngine;

public class MagicBeam_YSJ : MagicRoot_YSJ
{
    LineRenderer lineRenderer;
    LayerMask layerMask;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        layerMask = LayerMask.GetMask("EnemyHurtBox", "Obstacle");
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
        }
        else 
        {
            lineRenderer.SetPosition(1, transform.position + transform.right * Speed);
        }
    }

    private void Update()
    {
        
    }
}
