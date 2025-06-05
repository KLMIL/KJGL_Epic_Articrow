using System.Collections;
using UnityEngine;

namespace CKT
{
    public class DamageArea : MonoBehaviour
    {
        private void OnEnable()
        {
            StartCoroutine(DisableCoroutine());
        }

        IEnumerator DisableCoroutine()
        {
            yield return new WaitForSeconds(2f);
            gameObject.SetActive(false);
        }
    }
}