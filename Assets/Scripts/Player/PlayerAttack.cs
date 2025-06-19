using System.Collections;
using UnityEngine;
using YSJ;

namespace BMC
{
    public class PlayerAttack : MonoBehaviour
    {
        Rigidbody2D _rb;
        PlayerAnimator _playerAnimator; // 플레이어 애니메이터
        AttackSlash _attackSlash;

        [field: SerializeField] public int CurrentAttackStep { get; private set; }  // 현재 공격 단계
        int _maxAttackStep = 2;         // 최대 공격 단계

        float _comboTimer = 0f;
        float _comboInputWindow = 0.3f; // 공격 입력 타이밍 윈도우 (짧을 수록 공격 단계 후, 대기 시간이 짧아짐)
        bool _canNextCombo = false;     // 다음 콤보 입력 가능 여부
        bool _inputBuffered = false;    // 입력 버퍼 여부
        float _attackCoolDown = 0.35f;  // 모든 콤보 후, 대기 타임

        float _attackMoveDistance = 5f; // 공격 시, 플레이어가 전진하는 정도
        float _attackMoveTime = 0.02f;

        [field: SerializeField] public bool IsAttack { get; private set; }

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            IsAttack = false;
        }

        void Start()
        {
            _playerAnimator = GetComponent<PlayerAnimator>();
            _attackSlash = GetComponentInChildren<AttackSlash>();
            Managers.Input.OnLeftHandAction += Attack;
        }

        void Attack()
        {
            // 게임 일시 정지 시, 공격 불가
            if (GameManager.Instance.IsPaused || PlayerManager.Instance.PlayerStatus.IsDead)
                return;

            if (IsAttack)
            {
                if (_canNextCombo)
                    _inputBuffered = true;
                return;
            }

            Debug.Log("Attack 시작");
            StartAttackCoroutine();
            Debug.Log("Attack 종료");
        }

        public void StartAttackCoroutine()
        {
            StartCoroutine(AttackCoroutine());
        }

        IEnumerator AttackCoroutine()
        {
            if (CurrentAttackStep >= _maxAttackStep)
                yield break; // 이미 2단 콤보를 했으면 중단

            IsAttack = true;
            CurrentAttackStep++;
            _playerAnimator.CurrentState |= PlayerAnimator.State.Attack;

            _inputBuffered = false;

            // 공격 방향으로 플레이어 약간 이동
            AttackMove();

            // 단계에 맞는 공격 실행
            _attackSlash.Play(CurrentAttackStep);

            // 다음 콤보 입력 대기 시간
            _canNextCombo = CurrentAttackStep < _maxAttackStep;
            _comboTimer = 0;
            while (_canNextCombo && _comboTimer < _comboInputWindow)
            {
                _comboTimer += Time.deltaTime;
                if (_inputBuffered)
                {
                    _canNextCombo = false;
                    yield return null;
                    StartAttackCoroutine();
                    yield break;
                }
                yield return null;
            }

            if (Managers.Input.IsPressLeftHandAttack)
            {
                StartAttackCoroutine();
            }

            // 시간 내 입력 못했으면 초기화
            _canNextCombo = false;
            if (CurrentAttackStep >= _maxAttackStep) // 2단 콤보 되었을 경우
            {
                yield return new WaitForSeconds(_attackCoolDown);
            }
            CurrentAttackStep = 0; // 콤보 초기화

            if (Managers.Input.IsPressLeftHandAttack)
            {
                //yield return new WaitForSeconds(_attackCoolDown);
                StartAttackCoroutine();
                yield break;
            }
            //else
            //{
            //    //// 공격 끝
            //    //_playerAnimator.CurrentState &= ~PlayerAnimator.State.Attack;
            //    //IsAttack = false;
            //}

            // 공격 끝
            _playerAnimator.CurrentState &= ~PlayerAnimator.State.Attack;
            IsAttack = false;
        }

        public void AttackMove()
        {
            Vector3 mousePos = Managers.Input.MouseWorldPos;
            Vector2 dir = (mousePos - transform.position).normalized;
            StartCoroutine(AttackMoveCoroutine(dir, _attackMoveTime));
        }

        // 공격 시 이동 코루틴
        IEnumerator AttackMoveCoroutine(Vector2 dir, float waitTime)
        {
            _rb.linearVelocity = Vector2.zero;
            _rb.linearVelocity += dir * _attackMoveDistance;

            yield return new WaitForSeconds(waitTime);
            _rb.linearVelocity = Vector2.zero;
        }

        void OnDestroy()
        {
            Managers.Input.OnLeftHandAction -= Attack;
        }
    }
}