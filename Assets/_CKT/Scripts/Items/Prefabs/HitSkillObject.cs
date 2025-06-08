using System;
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
                foreach (Func<GameObject, IEnumerator> hitSkill in skillManager.HitSkillDict.Values)
                {
                    StartCoroutine(hitSkill(this.gameObject));
                }
            }
            else
            {
                Debug.LogError($"{this.name} skillManager is null");
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