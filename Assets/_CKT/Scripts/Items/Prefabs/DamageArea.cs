using System.Collections;
using UnityEngine;

namespace CKT
{
    public class DamageArea : MonoBehaviour
    {
        float _range = 2.2f;
        LayerMask _playerLayerMask;

        float _existTime = 2f;
        float _damage = 4f;
        float _damageGap = 0.2f;

        private void OnEnable()
        {
            _playerLayerMask = LayerMask.GetMask("Player");
            StartCoroutine(DisableCoroutine());
            StartCoroutine(TakeDamageCoroutine());
        }

        IEnumerator DisableCoroutine()
        {
            yield return new WaitForSeconds(_existTime);
            gameObject.SetActive(false);
        }
        
        IEnumerator TakeDamageCoroutine()
        {
            yield return null;

            while (this.gameObject.activeSelf)
            {
                RaycastHit2D[] hits = Physics2D.CircleCastAll(this.transform.position, _range, Vector2.up, 0, ~_playerLayerMask);
                for (int i = 0; i < hits.Length; i++)
                {
                    IDamagable iDamageable = hits[i].transform.GetComponent<IDamagable>();
                    if (iDamageable != null)
                    {
                        iDamageable.TakeDamage(_damage);
                    }
                }

                yield return new WaitForSeconds(_damageGap);
            }
        }
    }
}