using System.Collections;
using UnityEngine;

namespace CKT
{
    public class DamageArea : MonoBehaviour
    {
        private void OnEnable()
        {
            StartCoroutine(DisableCoroutine());
            StartCoroutine(TakeDamageCoroutine());
        }

        IEnumerator DisableCoroutine()
        {
            yield return new WaitForSeconds(2f);
            gameObject.SetActive(false);
        }
        
        IEnumerator TakeDamageCoroutine()
        {
            yield return null;

            while (this.gameObject.activeSelf)
            {
                RaycastHit2D[] hits = Physics2D.CircleCastAll(this.transform.position, 3, Vector2.up, 0);
                for (int i = 0; i < hits.Length; i++)
                {
                    IDamagable iDamageable = hits[i].transform.GetComponent<IDamagable>();
                    if (iDamageable != null)
                    {
                        iDamageable.TakeDamage(1);
                    }
                }

                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}