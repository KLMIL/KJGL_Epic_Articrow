using TMPro;
using UnityEngine;
using YSJ;

public class EnemyTakeDamage : MonoBehaviour, IDamagable
{
    TextMeshPro _damageTextPrefab;
    EnemyController ownerController;

    private void Start()
    {
        _damageTextPrefab = Managers.Resource.DamageText;
        ownerController = GetComponentInParent<EnemyController>();
    }

    public void TakeDamage(float damage)
    {
        TextMeshPro _damageTextInstance = Managers.TestPool.Get<TextMeshPro>(Define.PoolID.DamageText);
        _damageTextInstance.transform.position = transform.position;
        _damageTextInstance.text = damage.ToString();

        ownerController.FSM.isDamaged = true;
        ownerController.FSM.pendingDamage += damage;
    }
}
