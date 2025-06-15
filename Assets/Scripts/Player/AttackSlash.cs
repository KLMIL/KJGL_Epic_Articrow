using UnityEngine;
using System.Collections;
using YSJ;

namespace BMC
{
    public class AttackSlash : MonoBehaviour
    {
        Animator _anim;
        Coroutine _hitStopCoroutine;
        WaitForSeconds _hitStopTime = new WaitForSeconds(0.5f);

        void Start()
        {
            _anim = GetComponent<Animator>();
        }

        public void Play(int step)
        {
            switch(step)
            {
                case 1:
                    _anim.Play("AttackSlash1");
                    Debug.LogWarning("1단계 공격");
                    break;
                case 2:
                    _anim.Play("AttackSlash2");
                    Debug.LogWarning("2단계 공격");
                    break;
                default:
                    break;
            }
            transform.right = ((Vector3)Managers.Input.MouseWorldPos - transform.position);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if ((1 << collision.gameObject.layer) == LayerMask.GetMask("Monster"))
            {
                if (collision.TryGetComponent<IDamagable>(out IDamagable damagable))
                {
                    //if (_hitStopCoroutine == null)
                    //{
                    //    _hitStopCoroutine = StartCoroutine(HitStop());
                    //}
                    //Debug.Log("몬스터가 맞음");
                    damagable.TakeDamage(10);
                    PlayerManager.Instance.PlayerStatus.RegenerateMana(10);
                }
            }
        }

        //// 히트 스탑
        //IEnumerator HitStop()
        //{
        //    //Debug.Log("히트스탑");
        //    Time.timeScale = 0.75f;
        //    yield return _hitStopTime;
        //    _hitStopCoroutine = null;
        //    Time.timeScale = 1f;
        //}
    }
}