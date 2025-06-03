using System;
using System.Collections;
using UnityEngine;

namespace CKT
{
    public class Bullet : MonoBehaviour
    {
        private void OnEnable()
        {
            StartCoroutine(DisableCoroutine());
        }

        private void OnDisable()
        {
            //GameManager.Instance.Inventory.InvokeHitEffect(this.gameObject);
        }

        IEnumerator DisableCoroutine()
        {
            yield return new WaitForSeconds(1f);
            this.gameObject.SetActive(false);
        }
    }
}