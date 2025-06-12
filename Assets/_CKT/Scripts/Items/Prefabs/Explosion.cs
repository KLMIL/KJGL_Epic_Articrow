using System.Collections;
using UnityEngine;

namespace CKT
{
    public class Explosion : MonoBehaviour
    {
        GameObject[] _traceArray;
        
        float range = 1.5f;
        LayerMask _playerLayerMask;
        float _damage = 8f;
        float _disableTime = 1f;

        private void OnEnable()
        {
            //TOOD : 검댕 최적화하고 다시 호출하기
            //StartCoroutine(CreateTrace());
            
            _playerLayerMask = LayerMask.GetMask("Player");
            StartCoroutine(TakeDamageCoroutine());

            StartCoroutine(DisableCoroutine(_disableTime));
        }

        IEnumerator CreateTrace()
        {
            yield return null;
            _traceArray = Resources.LoadAll<GameObject>("ExplosionTraces");
            int random = Random.Range(0, _traceArray.Length);
            Instantiate(_traceArray[random], this.transform.position, Quaternion.identity);
        }

        IEnumerator TakeDamageCoroutine()
        {
            yield return null;

            RaycastHit2D[] hits = Physics2D.CircleCastAll(this.transform.position, range, Vector2.up, 0, ~_playerLayerMask);
            for (int i = 0; i < hits.Length; i++)
            {
                IDamagable iDamageable = hits[i].transform.GetComponent<IDamagable>();
                if (iDamageable != null)
                {
                    iDamageable.TakeDamage(_damage);
                }
            }
        }

        IEnumerator DisableCoroutine(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            this.gameObject.SetActive(false);
        }
    }
}