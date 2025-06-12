using TMPro;
using UnityEngine;
using YSJ;

public class EnemyTakeDamage : MonoBehaviour, IDamagable
{
    EnemyController ownerController;

    private void Start()
    {
        ownerController = GetComponentInParent<EnemyController>();
    }

    public void TakeDamage(float damage)
    {
        TextMeshPro damageText = Managers.TestPool.Get<TextMeshPro>(Define.PoolID.DamageText);
        damageText.text = damage.ToString();
        damageText.transform.position = transform.position;

        ownerController.FSM.isDamaged = true;
        ownerController.FSM.pendingDamage += damage;
    }
}
