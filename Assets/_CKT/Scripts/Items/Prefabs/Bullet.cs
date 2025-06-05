using System;
using System.Collections;
using UnityEngine;

namespace CKT
{
    public class Bullet : MonoBehaviour
    {
        float _bulletSpeed = 30f;

        private void OnEnable()
        {
            StartCoroutine(MoveCoroutine());
            StartCoroutine(DisableCoroutine());
        }

        private void OnDisable()
        {

        }

        IEnumerator MoveCoroutine()
        {
            while (this.gameObject.activeSelf)
            {
                transform.position += transform.up * _bulletSpeed * Time.deltaTime;
                yield return null;
            }
        }

        IEnumerator DisableCoroutine()
        {
            yield return new WaitForSeconds(1f);
            this.gameObject.SetActive(false);
        }
    }
}