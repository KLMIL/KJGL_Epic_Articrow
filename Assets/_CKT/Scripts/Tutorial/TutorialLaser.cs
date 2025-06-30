using BMC;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CKT
{
    public class TutorialLaser : MonoBehaviour
    {
        Coroutine restartCoroutine;

        private void OnDisable()
        {
            if (restartCoroutine != null)
            {
                StopCoroutine(restartCoroutine);
                restartCoroutine = null;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.isTrigger) return;
            if ((1<<collision.gameObject.layer) != LayerMask.GetMask("PlayerHurtBox")) return;
            if (BMC.PlayerManager.Instance.PlayerDash.IsDash) return;

            Debug.Log("Laser Hit");
            collision.GetComponent<IDamagable>().TakeDamage(100);

            restartCoroutine = StartCoroutine(RestartCoroutine());
        }

        IEnumerator RestartCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
            PlayerManager.Instance.Clear();
            YSJ.Managers.Scene.LoadScene(Define.SceneType.TutorialScene);
        }
    }
}