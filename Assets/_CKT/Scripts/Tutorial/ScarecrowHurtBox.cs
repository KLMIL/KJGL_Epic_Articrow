using UnityEngine;

namespace CKT
{
    public class ScarecrowHurtBox : MonoBehaviour, IDamagable
    {
        public void TakeDamage(float damage, Define.EnemyName attacker = Define.EnemyName.None)
        {
            TutorialManager.Instance.TutorialClear();
            Debug.Log("[ckt] ScarecrowHurtBox TutorialClear");
        }
    }
}