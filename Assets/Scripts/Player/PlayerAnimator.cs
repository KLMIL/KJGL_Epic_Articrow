using UnityEngine;

namespace YSJ
{
    public class PlayerAnimator : MonoBehaviour
    {
        Animator _anim;
        SpriteRenderer _spriteRenderer;
        CheckPlayerDirection _checkPlayerDirection;
        PlayerStatus _playerStatus;

        void Awake()
        {
            _playerStatus = GetComponent<PlayerStatus>();
            _checkPlayerDirection = GetComponent<CheckPlayerDirection>();
            _anim = GetComponent<Animator>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        void Update()
        {
            PlayAnimation();
        }

        public void PlayAnimation()
        {
            if (!_checkPlayerDirection || _checkPlayerDirection.CurrentDirection == CheckPlayerDirection.Direction.None)
            {
                return;
            }

            // Dead 상태
            if ((_playerStatus.CurrentState & PlayerStatus.PlayerState.Die) == PlayerStatus.PlayerState.Die)
            {
                FlipX();
                _anim.Play("Dead");
                return;
            }

            if ((_playerStatus.CurrentState & PlayerStatus.PlayerState.Attack) == PlayerStatus.PlayerState.Attack)
            {
                OnHurtAnimationEndEvent();
                return;
            }

            //Hurt 상태
            if ((_playerStatus.CurrentState & PlayerStatus.PlayerState.Hurt) == PlayerStatus.PlayerState.Hurt)
            {
                FlipX();
                _anim.Play("Hurt");
                return;
            }

            // Move 상태
            if ((_playerStatus.CurrentState & PlayerStatus.PlayerState.Move) == PlayerStatus.PlayerState.Move)
            {
                _anim.Play($"Move_{_checkPlayerDirection.CurrentDirection}");
                return;
            }

            // Idle 상태
            if ((_playerStatus.CurrentState & PlayerStatus.PlayerState.Idle) == PlayerStatus.PlayerState.Idle)
            {
                _anim.Play($"Idle_{_checkPlayerDirection.CurrentDirection}");
                return;
            }
        }

        public void AttackAnimation(int step)
        {
            _anim.Play($"Attack_{_checkPlayerDirection.CurrentDirection}{step}");
        }

        void FlipX()
        {
            _spriteRenderer.flipX = (_checkPlayerDirection.CurrentDirection == CheckPlayerDirection.Direction.Left) ? true : false;
        }

        // Hurt 애니메이션이 끝났을 때 호출되는 애니메이션 이벤트
        void OnHurtAnimationEndEvent() 
        {
            _playerStatus.CurrentState &= ~PlayerStatus.PlayerState.Hurt; // Hurt 상태 해제
        }
    }
}