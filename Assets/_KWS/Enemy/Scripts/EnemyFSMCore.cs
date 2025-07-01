using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyFSMCore
    {
        EnemyController _ownerController;
        List<EnemyBehaviourUnit> _behaviours;

        public string CurrentStateName { get; private set; } = "Spawned";


        #region Behaviour State Field 
        // 몬스터 피격 처리
        public bool isDamaged = false;
        public float pendingDamage = 0;
        public bool isDied = false;

        public float knockbackDistance = 0.1f;

        // 공격, 투사체 소환 등
        public int projectileFiredCount = 0;
        public float projectileIntervalTimer = 0;
        public Coroutine fireRoutine;

        public bool isSpawnedMite = false;

        public bool isRushAttacked = true;
        public bool isRushing = false;
        public Vector3 rushDirection = Vector3.zero;
        public float rushSpeedMultiply = 1f;
        public float rushDamageMultuply = 1f;

        public bool isContactDamageActive = false;
        public float currentActionDamageMultiply = 1.0f;

        // 접촉공격 쿨타임
        public float lastContactAttackTime = -Mathf.Infinity;
        public float contactAttackCooldown = 1.0f;

        // 이동
        public Vector3 randomMoveDirection = Vector3.zero;
        public float randomMoveChangeCooldown = 0f;

        // 몬스터 슈퍼아머
        public bool isSuperArmor = false;


        // TODO: 임시 변수 필드 -> EnemyStatusEffect 클래스로 분리하기
        public float enemyDamagedMultiply = 1f;
        public float enemyDamagedMultiplyRemainTime = 0f;

        // 인디케이터를 위한 공통 변수
        public float indicatorLength = 1f;
        public Vector2 IndicatorScale = Vector2.one;

        // TODO: 블랙홀 파츠를 위한 임시 필드
        public bool isGravitySurge = false;

        // AttackReady Action에서 저장할 플레이어(공격할) 위치
        public Vector2 AttackTargetPosition;

        // 인디케이터 위치 조정을 위한 피벗
        public Vector2 IndicatorPivot = Vector2.zero;
        #endregion



        public EnemyFSMCore(EnemyController ownerController, List<EnemyBehaviourUnit> behaviours)
        {
            _ownerController = ownerController;
            _behaviours = behaviours;
        }

        public void Update()
        {
            int idx = _behaviours.FindIndex(b => b.stateName == CurrentStateName);
            if (idx < 0) return;
            EnemyBehaviourUnit current = _behaviours[idx];

            if (HandleHardInterrupt(current)) return;
            HandleSoftInterrupt(current);
            HandleNoneInterrupt(current);
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