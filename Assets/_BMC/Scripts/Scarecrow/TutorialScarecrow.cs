using UnityEngine;

namespace BMC
{
    public class TutorialScarecrow : MonoBehaviour, IDamagable
    {
        Animator _anim;
        ShowDamageText _showDamageText;

        void Awake()
        {
            _anim = GetComponent<Animator>();
            _showDamageText = GetComponent<ShowDamageText>();
        }

        public void TakeDamage(float damage)
        {
            _anim.Play("Hurt");
            _showDamageText.Show(damage);
            TutorialManager.Instance.TutorialClear();
        }
    }
}