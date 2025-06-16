using System.Collections;
using UnityEngine;

namespace CKT
{
    public class GrabObject : MonoBehaviour
    {
        public int Level { get => _level; set => _level = value; }
        int _level = 0;
        float _grabScale = 8f;
        float _grabTime = 0.15f;
        float _minDistance = 1f;
        LayerMask _ignoreLayerMask;

        Transform _target;
        Coroutine _disableCoroutine;

        private void OnEnable()
        {
            _ignoreLayerMask = LayerMask.GetMask("Default", "Ignore Raycast", "Player", "BreakParts");
            StartCoroutine(Grab());
            _disableCoroutine = StartCoroutine(DisableCoroutine(0.2f));
        }

        private void OnDisable()
        {
            _level = 0;
            _target = null;
        }

        IEnumerator Grab()
        {
            yield return null;
            
            RaycastHit2D[] hits = Physics2D.CircleCastAll(this.transform.position, this.transform.localScale.x, Vector2.up, 0, ~_ignoreLayerMask);
            for (int i = 0; i < hits.Length; i++)
            {
                IDamagable iDamagable = hits[i].transform.GetComponent<IDamagable>();
                GrabObject grabObject = hits[i].transform.GetComponentInParent<GrabObject>();
                if ((iDamagable != null) && (grabObject == null))
                {
                    _target = hits[i].transform;
                    StartCoroutine(MoveCoroutine(_target));
                    StopCoroutine(_disableCoroutine);

                    //TODO : 사운드_HitGrab
                    yield break;
                }
            }
        }

        IEnumerator MoveCoroutine(Transform target)
        {
            this.transform.position = target.position;
            target.SetParent(this.transform);
            Vector3 startPos = target.position;

            Transform player = FindAnyObjectByType<BMC.PlayerManager>().transform;
            Vector3 playerPos = player.position;

            if ((player != null) && (target != null))
            {
                float distance = 0;
                while (distance < (_grabTime * _level))
                {
                    distance += Time.deltaTime;

                    float sqrDistance = (playerPos - this.transform.position).sqrMagnitude;
                    if (sqrDistance > (_minDistance * _minDistance))
                    {
                        Vector3 moveDir = (playerPos - startPos).normalized;
                        transform.position += moveDir * _grabScale * Time.deltaTime;
                    }

                    yield return null;
                }

                target.SetParent(null);
            }

            this.gameObject.SetActive(false);
        }

        IEnumerator DisableCoroutine(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            this.gameObject.SetActive(false);
        }
    }
}