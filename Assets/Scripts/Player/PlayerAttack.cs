using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using YSJ;

namespace BMC
{
    public class PlayerAttack : MonoBehaviour
    {
        Rigidbody2D _rb;
        PlayerAnimator _playerAnimator;
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
                // 콤보 입력 허용 구간일 때만 버퍼 허용
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
            // 해당 부분 없으면 꾹 누른 채로 공격 시, 계속 공격하며 죽는 모션 반복되는 현상 방지
            if (PlayerManager.Instance.PlayerStatus.IsDead)
                return;

            StartCoroutine(AttackCoroutine());
        }

        IEnumerator AttackCoroutine()
        {
            // 콤보 단계 증가 및 애니메이션 준비
            IsAttack = true;
            CurrentAttackStep++;
            _playerAnimator.CurrentState |= PlayerAnimator.State.Attack;
            _inputBuffered = false;

            // 공격 이동 & 이펙트(애니메이션) 실행
            AttackMove();
            _attackSlash.Play(CurrentAttackStep);

            // 다음 콤보 입력 대기
            _canNextCombo = CurrentAttackStep < _maxAttackStep;
            float timer = 0f;
            while (_canNextCombo && timer < _comboInputWindow)
            {
                timer += Time.deltaTime;
                if (_inputBuffered)
                {
                    // 클릭으로 버퍼링됐으면 바로 다음 단계
                    _canNextCombo = false;
                    yield return null;
                    StartAttackCoroutine();
                    yield break;
                }
                yield return null;
            }

            // 클릭 없이 입력 윈도우가 끝났고 계속 공격 누르고 있으면 Hold로 간주하고 자동 공격
            if (_canNextCombo && Managers.Input.IsPressLeftHandAttack)
            {
                _canNextCombo = false;
                yield return null;
                StartAttackCoroutine();
                yield break;
            }

            // 최종 단계까지 진행 후 대기
            if (CurrentAttackStep >= _maxAttackStep)
                yield return new WaitForSeconds(_attackCoolDown);

            // 콤보 리셋
            CurrentAttackStep = 0;
            _canNextCombo = false;

            // 상태 해제
            _playerAnimator.CurrentState &= ~PlayerAnimator.State.Attack;
            IsAttack = false;

            // (임시) 리셋 후, 계속 누르고 있으면 다시 시작
            if (Managers.Input.IsPressLeftHandAttack)
                StartAttackCoroutine();
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