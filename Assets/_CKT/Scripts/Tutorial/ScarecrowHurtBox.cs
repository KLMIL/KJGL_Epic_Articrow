using UnityEngine;

namespace CKT
{
    public class ScarecrowHurtBox : MonoBehaviour, IDamagable
    {
        bool _isFirstHit = false;

        public void TakeDamage(float damage)
        {
            if (!_isFirstHit)
            {
                _isFirstHit = true;
                TutorialManager.Instance.TutorialClear();
            }
        }
    }
}