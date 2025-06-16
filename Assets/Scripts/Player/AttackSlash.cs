using UnityEngine;
using System.Collections;
using YSJ;

namespace BMC
{
    public class AttackSlash : MonoBehaviour
    {
        Animator _playerAnim;
        Animator _anim;
        Coroutine _hitStopCoroutine;
        WaitForSeconds _hitStopTime = new WaitForSeconds(0.5f);
        CheckPlayerDirection _checkPlayerDirection;
        SpriteRenderer spriteRenderer;

        void Start()
        {
            _playerAnim = transform.parent.GetComponent<Animator>();
            spriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
            _anim = GetComponent<Animator>();
            _checkPlayerDirection = transform.parent.GetComponent<CheckPlayerDirection>();
        }

        public void Play(int step)
        {
            //int step = PlayerManager.Instance.PlayerAttack.CurrentAttackStep;
            switch (_checkPlayerDirection.CurrentDirection)
            {
                case CheckPlayerDirection.Direction.down:
                    Debug.LogWarning(step);
                    _playerAnim.Play($"Attack_Down{step}");
                    spriteRenderer.flipX = false;
                    break;
                case CheckPlayerDirection.Direction.up:
                    _playerAnim.Play($"Attack_Up{step}");
                    spriteRenderer.flipX = false;
                    break;
                case CheckPlayerDirection.Direction.right:
                    _playerAnim.Play($"Attack_Side{step}");
                    spriteRenderer.flipX = false;
                    break;
                case CheckPlayerDirection.Direction.left:
                    _playerAnim.Play($"Attack_Side{step}");
                    spriteRenderer.flipX = true;
                    break;
            }

            switch (step)
            {
                case 1:
                    _anim.Play("AttackSlash1");
                    Managers.Sound.PlaySFX(Define.SFX.Attack1);
                    Debug.LogWarning("1단계 공격");
                    break;
                case 2:
                    _anim.Play("AttackSlash2");
                    Debug.LogWarning("2단계 공격");
                    Managers.Sound.PlaySFX(Define.SFX.Attack2);
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
                    float damage = PlayerManager.Instance.PlayerAttack.CurrentAttackStep == 1 ? 10f : 15f;
                    damagable.TakeDamage(damage);
                    Managers.Sound.PlaySFX(Define.SFX.Slash);
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