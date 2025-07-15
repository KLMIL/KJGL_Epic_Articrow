using System.Collections;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        #region Field
        EnemyController ownerController;
        SpriteRenderer _spriteRenderer;
        Rigidbody2D rb;

        public float moveSpeedMultiply = 1.0f;
        Vector2 _currentDirection = Vector2.zero;

        float duration = 0f;
        float elapsedTime = 0f;
        public float wallCheckDistance = 1f;
        string moveType;
        bool inverse = false;

        public LayerMask wallLayerMask;      // 벽 감지용 (필수)
        #endregion


        #region LifeCycle Methods
        public void Init(EnemyController controller, SpriteRenderer renderer, Rigidbody2D rb)
        {
            ownerController = controller;
            _spriteRenderer = renderer;
            this.rb = rb;
        }

        private void FixedUpdate()
        {
            if (_currentDirection == Vector2.zero) return; // 움직일 방향이 없으면 중지

            elapsedTime += Time.fixedDeltaTime;
            switch (moveType)
            {
                case "Normal": 
                    NormalMove(); 
                    break;
                case "SimpleChase": 
                case "SmartChase": 
                    Chase(); 
                    break;
                case "SimpleMoveAway": 
                case "SmartMoveAway": 
                    MoveAway(); 
                    break;
                case "RushAttack": 
                    Rush(); 
                    break;
                case "Orbit": 
                    Orbit(); 
                    break;
            }
        }
        #endregion


        #region Private Methods
        private void NormalMove()
        {
            Vector2 origin = rb.position + _currentDirection * 0.1f; // 자기 자신 피함
            RaycastHit2D hit = Physics2D.Raycast(origin, _currentDirection, wallCheckDistance, LayerMask.GetMask("Obstacle", "Wall"));
            Debug.DrawRay(origin, _currentDirection * wallCheckDistance, Color.red);

            if (hit.collider != null)
            {
                ownerController.FSM.ForceToNextState();
                return;
            }

            if (elapsedTime < duration)
            {
                Vector2 nextPos = rb.position + _currentDirection * ownerController.Status.moveSpeed * moveSpeedMultiply * Time.fixedDeltaTime;
                FlipSpirte();
                rb.MovePosition(nextPos);
            }
            else
            {
                Stop();
            }
        }

        private void Chase()
        {
            Vector2 nextPos = rb.position + _currentDirection * ownerController.Status.moveSpeed * moveSpeedMultiply * Time.fixedDeltaTime;
            FlipSpirte();
            rb.MovePosition(nextPos);
        }

        private void MoveAway()
        {
            if (elapsedTime < duration)
            {
                Vector2 nextPos = rb.position + _currentDirection * ownerController.Status.moveSpeed * moveSpeedMultiply * Time.fixedDeltaTime;
                FlipSpirte();
                rb.MovePosition(nextPos);
            }
            else
            {
                Stop();
            }
        }

        private void Rush()
        {
            Vector2 rushDir = ownerController.FSM.rushDirection.normalized;
            wallCheckDistance = 1.0f;

            Vector2 origin = rb.position + rushDir * 0.1f;
            RaycastHit2D hit = Physics2D.Raycast(origin, rushDir, wallCheckDistance, LayerMask.GetMask("Obstacle"));
            if (hit.collider != null || elapsedTime > duration || ownerController.FSM.isRushAttacked)
            {
                Stop();
                ownerController.ForceToNextState();
                return;
            }

            Vector2 nextPos = rb.position + rushDir * ownerController.Status.moveSpeed * ownerController.FSM.rushSpeedMultiply * Time.fixedDeltaTime;
            FlipSpirte();
            rb.MovePosition(nextPos);
        }

        private void Orbit()
        {
            Vector2 origin = rb.position + _currentDirection * 0.1f; // 자기 자신 피함
            RaycastHit2D hit = Physics2D.Raycast(origin, _currentDirection, wallCheckDistance, LayerMask.GetMask("Obstacle"));
            Debug.DrawRay(origin, _currentDirection * wallCheckDistance, Color.red);

            if (hit.collider != null)
            {
                Stop();
                return;
            }

            // 이동에 상관없이 플레이어 방향 바라보도록 flipX 변경
            if (ownerController.Player != null)
            {
                float dx = ownerController.Player.position.x - rb.position.x;
                _spriteRenderer.flipX = dx > 0;
            }
            //FlipSpirte();

            // 이동할 위치 계산
            if (elapsedTime < duration)
            {
                Vector2 nextPos = rb.position + _currentDirection * ownerController.Status.moveSpeed * moveSpeedMultiply * Time.fixedDeltaTime;
                FlipSpirte();
                rb.MovePosition(nextPos);
            }
            else
            {
                Stop();
            }
        }

        private void FlipSpirte()
        {
            // 이동 전, 방향에 따라 스프라이트 좌우반전

            if (_spriteRenderer != null && _currentDirection.x != 0)
            {
                _spriteRenderer.flipX = inverse ? _currentDirection.x < 0 : _currentDirection.x > 0;
            }
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// 움직임 타입 및 방향 초기화
        /// </summary>
        /// <param name="direction"> 움직일 방향 </param>
        /// <param name="duration"> 움직이는 시간 </param>
        /// <param name="moveType"> 움직임 타입 </param>
        /// <param name="inverse"> 스프라이트 뒤집힘 여부 </param>
        public void MoveTo(Vector3 direction, float duration, string moveType, bool inverse)
        {
            _currentDirection = direction.normalized; // 안전을 위해 노말라이즈 수행
            this.duration = duration;
            this.moveType = moveType;
            this.inverse = inverse;

            elapsedTime = 0;
        }

        /// <summary>
        /// 모든 움직임 멈춤
        /// </summary>
        public void Stop()
        {
            _currentDirection = Vector2.zero;
        }

        /// <summary>
        /// 넉백 코루틴
        /// </summary>
        /// <param name="direction"> 넉백 방향 </param>
        /// <param name="distance"> 넉백 거리 </param>
        /// <param name="duration"> 넉백 총 시간 </param>
        /// <param name="steps"> 순차 넉백 횟수 </param>
        /// <returns></returns>
        public IEnumerator StepKnockback(Vector2 direction, float distance, float duration, int steps = 4)
        {
            // 넉백 코루틴

            direction = direction.normalized;
            float stepDist = distance / steps;
            float stepTime = duration / steps;

            // 넉백 상태 지정
            //ownerController.FSM.isKnockback = true;
            inverse = true;
            FlipSpirte();

            for (int i = 0; i < steps; i++)
            {
                Vector2 nextPos = rb.position + direction * stepDist;
                RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, stepDist, LayerMask.GetMask("Obstacle"));
                if (hit.collider != null)
                {
                    break;
                }

                rb.MovePosition(nextPos);
                yield return new WaitForSeconds(stepTime * 0.5f);
                yield return new WaitForSeconds(stepTime * 0.5f);
            }

            // 넉백 상태 복구
            //ownerController.FSM.isKnockback = false;
        }
        #endregion
    }
}