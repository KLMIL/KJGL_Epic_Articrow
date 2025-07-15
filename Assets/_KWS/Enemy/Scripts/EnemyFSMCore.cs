using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyFSMCore
    {
        EnemyController _ownerController;
        List<EnemyBehaviourUnit> _behaviours;

        #region Behaviour State: Common
        public string CurrentStateName { get; private set; } = "Spawned"; // 현재 상태 변수
        public bool isSuperArmor = false; // 몬스터 슈퍼아머 여부
        #endregion


        #region Behaviour State: Attack
        public float currentActionDamageMultiply = 1.0f; // 공격 대미지 배율

        public Coroutine fireRoutine; // ProjectileAttack Coroutine 저장

        public bool isSpawnedMite = false; // Jar의 패턴 수행 여부 검사

        // RushAttack 관련 Field
        public bool isRushAttacked = true;
        public bool isRushing = false;
        public Vector3 rushDirection = Vector3.zero;
        public float rushSpeedMultiply = 1f;
        public float rushDamageMultuply = 1f;
        #endregion


        #region Behaviour State: Move
        // 이동
        public Vector3 randomMoveDirection = Vector3.zero;
        public float randomMoveChangeCooldown = 0f;
        public bool isRandomMoving = false;

        public bool isChasing = false;      // Chase Action 시작
        public bool isBypassingChase = false;    // Chase 중 장애물 우회상태
        public Vector2 bypassDirection = Vector2.zero; // 우회 경로

        public bool isMovingAway = false;
        public bool isBypassingMoveAway = false;
        public Vector2 moveAwayBypassingDirection = Vector2.zero;

        public bool isOrbit = false;
        public Vector2 orbitDir = Vector2.zero;
        public float orbitChangeTime = -Mathf.Infinity;
        #endregion


        #region Behaviour State: State
        public bool isSpawnEffect = false; // 스폰될때 마법진 소환했는지 여부

        // 인디케이터를 위한 공통 변수
        public Vector2 AttackTargetPosition; // AttackReady Action에서 저장할 공격할 위치(일반적으로 플레이어 위치)

        // 몬스터 피격 처리
        public bool isDamaged = false;
        public bool isDied = false;
        public float knockbackDistance = 0.1f;
        #endregion


        public EnemyFSMCore(EnemyController ownerController, List<EnemyBehaviourUnit> behaviours)
        {
            _ownerController = ownerController;
            _behaviours = behaviours;
        }

        public void Update()
        {
            // 디버깅용 String 변경
            _ownerController._currentState = CurrentStateName;

            int idx = _behaviours.FindIndex(b => b.stateName == CurrentStateName);
            if (idx < 0) return;
            EnemyBehaviourUnit current = _behaviours[idx];

            if (HandleHardInterrupt(current)) return;
            HandleSoftInterrupt(current);
            HandleNoneInterrupt(current);

            //Debug.LogError($"[{Time.time}]: Current Behaviour: {current.stateName}");
        }


        private bool HandleHardInterrupt(EnemyBehaviourUnit current)
        {
            int currentIdx = _behaviours.FindIndex(b => b.stateName == CurrentStateName);

            if (current.interruptType == InterruptType.Hard)
            {
                for (int i = 0; i < currentIdx; i++)
                {
                    var candidate = _behaviours[i];
                    if (candidate.interruptType == InterruptType.Hard &&
                        candidate.condition.IsMet(_ownerController))
                    {
                        ChangeState(candidate.stateName);
                        HandleNoneInterrupt(current);
                        return true;
                    }
                }
                HandleNoneInterrupt(current);
                return true;
            }

            int hardIdx = _behaviours.FindIndex(
                    b => b.interruptType == InterruptType.Hard &&
                         b.condition.IsMet(_ownerController)
                );
            if (hardIdx >= 0)
            {
                ChangeState(_behaviours[hardIdx].stateName);
                return true;
            }
            return false;
        }

        private void HandleSoftInterrupt(EnemyBehaviourUnit current)
        {
            int softIdx = -1;
            if (current.interruptType != InterruptType.Soft)
            {
                softIdx = _behaviours.FindIndex(
                        b => b.interruptType == InterruptType.Soft &&
                             b.condition.IsMet(_ownerController)
                    );
            }
            if (softIdx >= 0)
            {
                ChangeState(_behaviours[softIdx].stateName);
            }
        }

        private void HandleNoneInterrupt(EnemyBehaviourUnit current)
        {
            current.elapsedTime += Time.deltaTime;
            if (current.elapsedTime <= current.duration)
            {
                current.action?.Act(_ownerController);
            }
            else
            {
                ChangeState(current.nextStateName);
            }
        }


        public void ChangeState(string nextStateName)
        {
            //Debug.Log($"[{Time.time}] State Change: {nextStateName}");

            if (CurrentStateName == nextStateName) return; // 같은 상태라면 애니메이션 갱신 X

            int prevIdx = _behaviours.FindIndex(b => b.stateName == CurrentStateName);
            int nextIdx = _behaviours.FindIndex(b => b.stateName == nextStateName);

            if (prevIdx >= 0)
            {
                _behaviours[prevIdx].action?.OnExit(_ownerController);
            }

            CurrentStateName = nextStateName;

            if (nextIdx >= 0)
            {
                _behaviours[nextIdx].ResetTimer();
                _behaviours[nextIdx].action?.OnEnter(_ownerController);
                _ownerController.PlayAnimationOnce(_behaviours[nextIdx].animationName);
            }
        }

        public void ForceToNextState()
        {
            int idx = _behaviours.FindIndex(b => b.stateName == CurrentStateName);

            if (idx >= 0)
            {
                string next = _behaviours[idx].nextStateName;
                ChangeState(next);
            }
        }
    }
}