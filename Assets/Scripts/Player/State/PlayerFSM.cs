using System.Collections.Generic;
using UnityEngine;
using YSJ;

namespace BMC
{
    public enum PlayerState
    {
        None,
        Idle,
        Move,
        Dash,
        Attack,
        Hit,
        Die,
    }

    public class PlayerFSM : MonoBehaviour
    {
        SpriteRenderer _spriteRenderer;
        CheckPlayerDirection _checkPlayerDirection;

        [field: SerializeField] public State CurrentState { get; set; }     // 현재 상태
        Dictionary<PlayerState, State> _stateDictionary = new Dictionary<PlayerState, State>();

        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _checkPlayerDirection = GetComponent<CheckPlayerDirection>();

            _stateDictionary.Add(PlayerState.Idle, gameObject.GetComponent<PlayerIdleState>());
            _stateDictionary.Add(PlayerState.Move, gameObject.GetComponent<PlayerMoveState>());
            _stateDictionary.Add(PlayerState.Dash, gameObject.GetComponent<PlayerDashState>());
            _stateDictionary.Add(PlayerState.Attack, gameObject.GetComponent<PlayerAttackState>());

            SetState(PlayerState.Idle); // 초기 상태 설정
        }

        void Update()
        {
            Do();
        }

        public void SetState(PlayerState state)
        {
            if (CurrentState != null)
                CurrentState.OnStateExit();
            CurrentState = _stateDictionary[state];
            CurrentState.OnStateEnter();
        }

        public void Do()
        {
            if (CurrentState != null)
                CurrentState.OnStateUpdate();
        }

        public void Flip()
        {
            _spriteRenderer.flipX = (_checkPlayerDirection.CurrentDirection == CheckPlayerDirection.Direction.Left) ? true : false;
        }

        #region 각 상태로의 전환 검사 및 전환
        // Idle 전환 검사
        public void CheckAndSetIdle()
        {
            if (Managers.Input.MoveInput == Vector2.zero)
                SetState(PlayerState.Idle);
        }

        // Move 전환 검사
        public void CheckAndSetMove()
        {
            if (Managers.Input.MoveInput != Vector2.zero)
                SetState(PlayerState.Move);
        }

        // Dash 전환 검사
        public void CheckAndSetDash()
        {
            if(Managers.Input.IsPressDash)
                SetState(PlayerState.Dash);
        }

        // Attack 전환 검사
        public void CheckAndSetAttack()
        {
            if (Managers.Input.IsPressLeftHandAttack)
                SetState(PlayerState.Attack);
        }
        #endregion
    }
}