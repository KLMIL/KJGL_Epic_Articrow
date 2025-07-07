using System.Collections;
using UnityEngine;
using BMC;

namespace CKT
{
    public class TutorialLaser : MonoBehaviour
    {
        Coroutine _restartCoroutine;

        void OnDisable()
        {
            if (_restartCoroutine != null)
            {
                StopCoroutine(_restartCoroutine);
                _restartCoroutine = null;
            }
        }

        void OnTriggerStay2D(Collider2D collision)
        {
            if ((1 << collision.gameObject.layer) == LayerMask.GetMask("PlayerHurtBox"))
            {
                if (PlayerManager.Instance.PlayerDash.IsDash)
                    return;

                if (collision.TryGetComponent<IDamagable>(out IDamagable damagable))
                {
                    damagable.TakeDamage(PlayerManager.Instance.PlayerStatus.MaxHealth);
                    _restartCoroutine = StartCoroutine(RestartCoroutine());
                }
            }
        }

        IEnumerator RestartCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
            PlayerManager.Instance.Clear();
            YSJ.Managers.Scene.LoadScene(Define.SceneType.TutorialScene);
        }
    }
}