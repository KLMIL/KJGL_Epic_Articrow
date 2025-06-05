using UnityEngine;

/*
 * 특수 공격들 정의
 * Summon: 프리펩에 해당하는 적 개체 소환 
 */
public enum SpecialAttackMode { Summon };
[CreateAssetMenu(
    fileName = "SpecialAttackAction",
    menuName = "Enemy/Action/Attack/Special Attack"
)]
public class SpecialAttackActionSO: EnemyActionSO
{
    public SpecialAttackMode specialAttackMode = SpecialAttackMode.Summon;

    [Header("Summon")]
    public GameObject summoningPrefab;
    public int spawnCount = 3;
    public float spawnRadius = 1f;


    public override void Act(EnemyController controller)
    {
        switch (specialAttackMode)
        {
            case SpecialAttackMode.Summon:
                Summon(controller);
                break;
        }
    }

    public override void OnExit(EnemyController controller)
    {
        switch (specialAttackMode)
        {
            case SpecialAttackMode.Summon:
                controller.isSpawnedMite = false;
                break;
        }
    }


    private void Summon(EnemyController controller)
    {
        if (controller.isSpawnedMite) return;
        controller.isSpawnedMite = true;

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 rand = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPos = controller.transform.position + new Vector3(rand.x, 0, rand.y);
            Instantiate(summoningPrefab, spawnPos, Quaternion.identity, controller.transform);
        }
    }
}