using TMPro;
using UnityEngine;
using YSJ;

public class EnemyTakeDamage : MonoBehaviour, IDamagable
{
    TextMeshPro _damageTextPrefab;
    EnemyController ownerController;

    private void Start()
    {
        _damageTextPrefab = Managers.Resource.Load<TextMeshPro>("Text/DamageText");
        ownerController = GetComponentInParent<EnemyController>();
    }

    public void TakeDamage(float damage)
    {
        TextMeshPro _damageTextInstance = Instantiate(_damageTextPrefab, transform.position, Quaternion.identity);
        _damageTextInstance.text = damage.ToString();

        ownerController.FSM.isDamaged = true;
        ownerController.FSM.pendingDamage += damage;
    }
}
