using System.Collections;
using UnityEngine;

namespace CKT
{
    public class GrabObject : MonoBehaviour
    {
        float _moveSpeed = 1.3f;
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
                if (iDamagable != null)
                {
                    _target = hits[i].transform;
                    StartCoroutine(MoveCoroutine(_target));
                    break;
                }
            }
        }

        IEnumerator MoveCoroutine(Transform target)
        {
            target.SetParent(this.transform);
            target.position = this.transform.position;
            Vector3 startPos = this.transform.position;

            Transform player = FindAnyObjectByType<YSJ.PlayerController>().transform;
            Vector3 playerPos = player.position;

            if ((player != null) && (target != null))
            {
                float sqrDistance = (playerPos - this.transform.position).sqrMagnitude;
                float cur = 0;

                while (sqrDistance > 1)
                {
                    cur += Time.deltaTime;

                    transform.position = Vector3.Lerp(startPos, playerPos, (cur * _moveSpeed));
                    sqrDistance = (playerPos - this.transform.position).sqrMagnitude;
                    yield return null;
                }

                target.SetParent(null);
            }

            this.gameObject.SetActive(false);
        }
    }
}