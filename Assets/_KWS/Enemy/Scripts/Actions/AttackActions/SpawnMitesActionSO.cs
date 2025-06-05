using UnityEngine;

[CreateAssetMenu(fileName = "SpawnMitesAction", menuName = "Enemy/Action/Attack/Spawn Mites")]
public class SpawnMitesActionSO : EnemyActionSO
{
    public GameObject mitePrefab;
    public int spawnCount = 3;
    public float spawnRadius = 1f;

    public override void Act(EnemyController controller)
    {
        if (controller.isSpawnedMite) return;

        controller.isSpawnedMite = true;

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 rand = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPos = controller.transform.position + new Vector3(rand.x, 0, rand.y);
            Instantiate(mitePrefab, spawnPos, Quaternion.identity, controller.transform);
        }
        //controller.Animation.Play("Spawn");
    }
}
