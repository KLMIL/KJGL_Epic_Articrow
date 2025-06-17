using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YSJ;

namespace BMC
{
    public class PlayerAttackState : State
    {
        Animator _anim;
        Rigidbody2D _rb;
        PlayerFSM _playerFSM;
        CheckPlayerDirection _checkPlayerDirection;
        AttackSlash _attackSlash;

        Coroutine _attackCoroutine;

        [field: SerializeField] public int CurrentAttackStep { get; private set; }  // 현재 공격 단계
        int _maxAttackStep = 2;         // 최대 공격 단계

        float _comboTimer = 0f;
        float _comboInputWindow = 0.3f; // 공격 입력 타이밍 윈도우 (짧을 수록 1단계 공격 후, 대기 시간이 짧아짐)
        bool _canNextCombo = false;     // 다음 콤보 입력 가능 여부
        bool _inputBuffered = false;    // 입력 버퍼 여부
        float _attackCoolDown = 0.3f;   // 모든 콤보 후, 대기 타임

        float _attackMoveDistance = 0.75f; // 공격 시, 플레이어가 전진하는 정도

        [field: SerializeField] public bool IsAttack { get; private set; }

        void Start()
        {
            _anim = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
            _playerFSM = GetComponent<PlayerFSM>();
            _checkPlayerDirection = GetComponent<CheckPlayerDirection>();
            _attackSlash = GetComponentInChildren<AttackSlash>();

            Managers.Input.OnLeftHandAction += Attack;
        }

        public override void OnStateEnter()
        {
            Attack();
        }

        public override void OnStateUpdate()
        {
            if (!IsAttack && _attackCoroutine == null)
                _playerFSM.SetState(PlayerState.Idle);
        }

        public override void OnStateExit()
        {
            Debug.Log("Attack 종료");
            IsAttack = false;
            CurrentAttackStep = 0;
        }

        void Attack()
        {
            //// 게임 일시 정지 시, 공격 불가
            //if (GameManager.Instance.IsPaused)
            //    return;

            if (!IsAttack)
            {
                StartAttackCoroutine();
            }
            else if (_canNextCombo)
            {
                _inputBuffered = true;
            }
        }

        void StartAttackCoroutine()
        {
            if (_attackCoroutine != null || _maxAttackStep <= CurrentAttackStep) 
                return;

            _attackCoroutine = StartCoroutine(AttackCoroutine());
        }

        IEnumerator AttackCoroutine()
        {
            IsAttack = true;
            while (CurrentAttackStep < _maxAttackStep)
            {
                CurrentAttackStep++;
                _inputBuffered = false;

                // 공격 방향으로 살짝 이동
                Vector2 dir = (Managers.Input.MouseWorldPos - (Vector2)transform.position).normalized;
                _rb.linearVelocity = dir * _attackMoveDistance;
                yield return new WaitForSeconds(0.05f); // 단발 반동
                _rb.linearVelocity = Vector2.zero;

                // 공격 모션 재생
                _attackSlash.Play(CurrentAttackStep);
                PlayAttackAnimation(CurrentAttackStep);

                // 콤보 입력 대기
                _canNextCombo = CurrentAttackStep < _maxAttackStep;
                float timer = 0f;
                while (_canNextCombo && timer < _comboInputWindow)
                {
                    if (_inputBuffered)
                    {
                        yield return null; // 다음 루프에서 공격
                        break;
                    }
                    timer += Time.deltaTime;
                    yield return null;
                }

                if (!_inputBuffered) 
                    break;
            }

            yield return new WaitForSeconds(_attackCoolDown);
            IsAttack = false;
            _attackCoroutine = null;
            _playerFSM.SetState(PlayerState.Idle);
        }

        void PlayAttackAnimation(int step)
        {
            if (step > _maxAttackStep && 0<step)
                return;

            _anim.StopPlayback();
            _playerFSM.Flip();
            string direction = _checkPlayerDirection.CurrentDirection switch
            {
                CheckPlayerDirection.Direction.Down => "Down",
                CheckPlayerDirection.Direction.Up => "Up",
                _ => "Side"
            };
            _anim.Play($"Attack_{direction}{step}");
            Debug.LogWarning($"Attack_{direction}{step} 재생");
        }

        void OnDestroy()
        {
            Managers.Input.OnLeftHandAction -= Attack;
        }
    }
}