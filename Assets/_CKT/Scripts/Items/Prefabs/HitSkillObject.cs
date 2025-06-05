using System.Collections;
using UnityEngine;

namespace CKT
{
    public class HitSkillObject : MonoBehaviour
    {
        public void HitSkill(SkillManager skillManager)
        {
            if (skillManager != null)
            {
                skillManager.InvokeHitSkillEvent(gameObject);
                IEnumerator iEnumerator = skillManager.InvokeHitSkillIEnumerator(gameObject);
                if (iEnumerator != null)
                {
                    StartCoroutine(iEnumerator);
                }
            }

            StartCoroutine(DisableCoroutine());
        }

        IEnumerator DisableCoroutine()
        {
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }
    }
}