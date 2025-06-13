using System.Collections;
using UnityEngine;

namespace CKT
{
    public class GrabObject : MonoBehaviour
    {
        float _minSqrDistance = 2f;
        float _moveSpeed = 10f;
        LayerMask _playerLayerMask;

        Transform _target;

        private void OnEnable()
        {
            _playerLayerMask = LayerMask.GetMask("Player");
        }

        private void OnDisable()
        {
            _target = null;
        }

        private void FixedUpdate()
        {
            if (_target == null)
            {
                Grab();
            }
        }

        void Grab()
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(this.transform.position, this.transform.localScale.x, Vector2.up, 0, ~_playerLayerMask);
            for (int i = 0; i < hits.Length; i++)
            {
                IDamagable iDamagable = hits[i].transform.GetComponent<IDamagable>();
                GrabObject grabObject = hits[i].transform.GetComponentInParent<GrabObject>();
                if ((iDamagable != null) && (grabObject == null))
                {
                    _target = hits[i].transform;
                    StartCoroutine(MoveCoroutine(_target));

                    //TODO : 사운드_HitGrab
                    break;
                }
            }
        }

        IEnumerator MoveCoroutine(Transform target)
        {
            target.SetParent(this.transform);
            target.position = this.transform.position;
            Vector3 startPos = this.transform.position;

            Transform player = FindAnyObjectByType<BMC.PlayerController>().transform;
            Vector3 playerPos = player.position;

            if ((player != null) && (target != null))
            {
                float sqrDistance = float.MaxValue;
                while (sqrDistance > _minSqrDistance)
                {
                    Vector3 moveDir = (playerPos - startPos).normalized;
                    transform.position += moveDir * _moveSpeed * Time.deltaTime;
                    sqrDistance = (playerPos - this.transform.position).sqrMagnitude;
                    yield return null;
                }

                target.SetParent(null);
            }

            this.gameObject.SetActive(false);
        }
    }
}