using UnityEngine;

public class EnemyTakeDamage : MonoBehaviour, IDamagable
{
    EnemyController ownerController;

    private void Awake()
    {
        ownerController = GetComponent<EnemyController>();
    }

    // FSM 상태 호출
    public void TakeDamage(float damage)
    {
        ownerController.Status.healthPoint -= damage;
        ownerController.isDamaged = true;
    }
}
