using System.Collections;
using UnityEngine;

namespace CKT
{
    public class Explosion : MonoBehaviour
    {
        LayerMask _playerLayerMask;

        private void OnEnable()
        {
            _playerLayerMask = LayerMask.GetMask("Player");
            StartCoroutine(TakeDamageCoroutine());
        }

        IEnumerator TakeDamageCoroutine()
        {
            yield return null;

            RaycastHit2D[] hits = Physics2D.CircleCastAll(this.transform.position, 2, Vector2.up, 0, ~_playerLayerMask);
            for (int i = 0; i < hits.Length; i++)
            {
                IDamagable iDamageable = hits[i].transform.GetComponent<IDamagable>();
                if (iDamageable != null)
                {
                    iDamageable.TakeDamage(10);
                }
            }
        }
    }
}