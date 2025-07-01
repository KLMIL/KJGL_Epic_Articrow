using UnityEngine;

public class MagicGuide_YSJ : MagicRoot_YSJ
{
    LineRenderer lineRenderer;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Start()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + transform.right * Speed);
    }

    private void Update()
    {
        CountLifeTime(ownerArtifact, gameObject);
    }
}
