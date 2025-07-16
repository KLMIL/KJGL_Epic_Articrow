using UnityEngine;

namespace CKT
{
    public class ScarecrowHurtBox : MonoBehaviour, IDamagable
    {
        public void TakeDamage(float damage, Transform attacker = null)
        {
            TutorialManager.Instance.TutorialClear();
            Debug.Log("[ckt] ScarecrowHurtBox TutorialClear");
        }
    }
}