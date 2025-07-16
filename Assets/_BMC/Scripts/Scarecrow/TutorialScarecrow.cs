using UnityEngine;

namespace BMC
{
    public class TutorialScarecrow : MonoBehaviour, IDamagable
    {
        [SerializeField] int _id; // 튜토리얼 허수아비 ID

        Animator _anim;
        ShowDamageText _showDamageText;

        void Awake()
        {
            _anim = GetComponent<Animator>();
            _showDamageText = GetComponent<ShowDamageText>();
        }

        public void TakeDamage(float damage, Transform attacker = null)
        {
            _anim.Play("Hurt");
            _showDamageText.Show(damage);

            if (_id == 1)
            {
                TutorialManager.Instance.TutorialClear();
            }
        }
    }
}