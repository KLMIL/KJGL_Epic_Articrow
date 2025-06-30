using UnityEngine;

namespace CKT
{
    public class ScarecrowHurtBox : MonoBehaviour, IDamagable
    {
        public void TakeDamage(float damage)
        {
            TutorialManager.Instance.TutorialClear();
            Debug.Log("[ckt] ScarecrowHurtBox TutorialClear");
        }
    }
}