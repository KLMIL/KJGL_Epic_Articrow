using UnityEngine;

public class EnemyTakeDamage : MonoBehaviour, IDamagable
{
    EnemyController ownerController;

    private void Start()
    {
        ownerController = GetComponentInParent<EnemyController>();
    }

    public void TakeDamage(float damage)
    {
        ownerController.isDamaged = true;
        ownerController.pendingDamage += damage;
    }
}
