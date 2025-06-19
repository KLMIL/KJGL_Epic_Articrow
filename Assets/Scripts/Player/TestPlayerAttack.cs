using UnityEngine;
using YSJ;
using System.Collections;

namespace BMC
{
    public class TestPlayerAttack : MonoBehaviour
    {
        Rigidbody2D _rb;
        PlayerAnimator _playerAnimator; // 플레이어 애니메이터
        AttackSlash _attackSlash;

        [field: SerializeField] public int CurrentAttackStep { get; private set; }  // 현재 공격 단계
        int _maxAttackStep = 2;         // 최대 공격 단계

        public Animator ani;
        public int combo;
        public bool IsCanAttack;

        [Header("공격 전진")]
        float _attackMoveDistance = 5f; // 공격 시, 플레이어가 전진하는 정도
        float _attackMoveTime = 0.02f;

        void Start()
        {
            ani = GetComponent<Animator>();
        }

        private void Update()
        {
            Combos();
        }

        public void Start_Combo()
        {
            IsCanAttack = false;
            if (combo < _maxAttackStep)
            {
                combo++;
            }
        }

        public void Finish_Ani()
        {
            IsCanAttack = false;
            combo = 0;
        }

        public void Combos()
        {
            if (Input.GetMouseButton(0) && !IsCanAttack)
            {
                IsCanAttack = true;
                _playerAnimator.CurrentState |= PlayerAnimator.State.Attack;
                
                // 공격 방향으로 플레이어 약간 이동
                AttackMove();

                // 단계에 맞는 공격 실행
                CurrentAttackStep++;
                _attackSlash.Play(CurrentAttackStep);
            }
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
    }
}