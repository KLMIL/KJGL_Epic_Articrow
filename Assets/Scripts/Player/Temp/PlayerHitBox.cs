using UnityEngine;
using YSJ;

namespace BMC
{
    public class PlayerHitBox : MonoBehaviour
    {
        public Animator Anim { get; private set; }
        PlayerAnimator _playerAnimator;

        void Start()
        {
            _playerAnimator = GetComponentInParent<PlayerAnimator>();
            Anim = GetComponent<Animator>();
        }

        public void Play(int step)
        {
            //int step = PlayerManager.Instance.PlayerAttack.CurrentAttackStep;
            _playerAnimator.AttackAnimation(step);
            switch (step)
            {
                case 1:
                    Anim.Play("AttackSlash1");
                    //Managers.Sound.PlaySFX(Define.SFX.Attack1);
                    Debug.LogWarning("1단계 공격");
                    break;
                case 2:
                    Anim.Play("AttackSlash2");
                    Debug.LogWarning("2단계 공격");
                    //Managers.Sound.PlaySFX(Define.SFX.Attack2);
                    break;
                default:
                    break;
            }
            transform.right = ((Vector3)Managers.Input.MouseWorldPos - transform.position);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            //TODO : 콜리전 정리하면서 같이 정리하기
            if (!collision.isTrigger) return;
            
            if ((1 << collision.gameObject.layer) == LayerMask.GetMask("EnemyHurtBox"))
            {
                if (collision.TryGetComponent<IDamagable>(out IDamagable damagable))
                {
                    //if (_hitStopCoroutine == null)
                    //{
                    //    _hitStopCoroutine = StartCoroutine(HitStop());
                    //}
                    //Debug.Log("몬스터가 맞음");
                    
                    //// 마나 흡수
                    //ManaEnergy spirit = Managers.TestPool.Get<ManaEnergy>(Define.PoolID.ManaEnergy);
                    //spirit.transform.position = collision.transform.position;
                    //spirit.Init();
                    //spirit.MoveToSlider();

                    //float damage = PlayerManager.Instance.PlayerAttack.CurrentAttackStep == 1 ? 8f : 12f;
                    //damagable.TakeDamage(damage);
                    //Managers.Sound.PlaySFX(Define.SFX.Slash);
                    //PlayerManager.Instance.PlayerStatus.RegenerateMana(damage);
                }
            }
        }
    }
}