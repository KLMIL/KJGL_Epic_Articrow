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
                for (int i = 0; i < skillManager.HitSkillList.Count; i++)
                {
                    StartCoroutine(skillManager.HitSkillList[i](this.gameObject));
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