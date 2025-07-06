using UnityEngine;

public class MagicGuide_YSJ : MagicRoot_YSJ
{
    LineRenderer lineRenderer;
    public GameObject SkillAttackPrefab;
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

    public override void CountLifeTime(Artifact_YSJ ownerArtifact, GameObject Attack)
    {
        if (elapsedTime < LifeTime)
        {
            elapsedTime += Time.deltaTime;
        }
        else
        {
            GameObject spawnedAttack = Instantiate(SkillAttackPrefab, transform.position, transform.rotation);
            spawnedAttack.GetComponent<MagicRoot_YSJ>().SkillAttackInitialize(ownerArtifact);
            Destroy(gameObject);
        }
    }

    public override void SkillAttackInitialize(Artifact_YSJ ownerArtifact)
    {
        base.SkillAttackInitialize(ownerArtifact);
        LifeTime = ownerArtifact.skillStatus.Current_AttackStartDelay;
    }
}
