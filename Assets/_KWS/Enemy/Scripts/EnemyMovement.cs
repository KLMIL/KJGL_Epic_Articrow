using System.Collections;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        EnemyController ownerController;
        Rigidbody2D rb;
        public SpriteRenderer _spriteRenderer;

        public float moveSpeedMultiply = 1.0f;
        Vector2 _currentDirection = Vector2.zero;

        float duration = 0f;
        float elapsedTime = 0f;
        public float wallCheckDistance = 0.5f;
        string moveType;
        bool inverse = false;

        public LayerMask wallLayerMask;      // 벽 감지용 (필수)

        private void Awake()
        {
            ownerController = GetComponent<EnemyController>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            //_spriteRenderer = ownerController.SpriteRenderer;
            //if (_spriteRenderer == null)
            //{
            //    Debug.LogError($"{gameObject.name} Sprite renderer 할당 오류");
            //}
        }

        public void Init(SpriteRenderer renderer)
        {
            _spriteRenderer = renderer;
        }

        private void FixedUpdate()
        {
            if (_currentDirection == Vector2.zero) return;
            // 좌우 이동 방향에 따라 flipX 결정

            //Debug.Log($"움직일 수 있나? {ownerController.FSM.isGravitySurge}");
            if (ownerController.FSM.isGravitySurge) return; // 임시로 끌려갈때는 움직임 X


            elapsedTime += Time.fixedDeltaTime;

            if (moveType == "Normal")
            {
                Vector2 origin = rb.position + _currentDirection * 0.1f; // 자기 자신 피함
                RaycastHit2D hit = Physics2D.Raycast(origin, _currentDirection, wallCheckDistance, wallLayerMask);
                Debug.DrawRay(origin, _currentDirection * wallCheckDistance, Color.red);

                if (hit.collider != null)
                {
                    Stop();
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
            else if (moveType == "SimpleChase" || moveType == "SmartChase")
            {
                Vector2 nextPos = rb.position + _currentDirection * ownerController.Status.moveSpeed * moveSpeedMultiply * Time.fixedDeltaTime;
                FlipSpirte();
                rb.MovePosition(nextPos);
                // (단발 이동 원한다면 Stop();)
            }
            else if (moveType == "SimpleMoveAway" || moveType == "SmartMoveAway")
            {
                //Debug.LogError("Here?");

                if (elapsedTime < duration)
                {
                    //Vector2 origin = rb.position + _currentDirection * 0.1f;
                    //RaycastHit2D hit = Physics2D.Raycast(origin, _currentDirection, wallCheckDistance, wallLayerMask);
                    //if (hit.collider != null)
                    //{
                    //    Stop();
                    //    return;
                    //}
                    Vector2 nextPos = rb.position + _currentDirection * ownerController.Status.moveSpeed * moveSpeedMultiply * Time.fixedDeltaTime;
                    FlipSpirte();
                    rb.MovePosition(nextPos);
                }
                else
                {
                    Stop();
                }
            }
            else if (moveType == "RushAttack")
            {
                Vector2 rushDir = ownerController.FSM.rushDirection.normalized;
                wallCheckDistance = 1.0f;

                Vector2 origin = rb.position + rushDir * 0.1f;
                RaycastHit2D hit = Physics2D.Raycast(origin, rushDir, wallCheckDistance, wallLayerMask);
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
            else if (moveType == "Orbit")
            {
                Vector2 origin = rb.position + _currentDirection * 0.1f; // 자기 자신 피함
                RaycastHit2D hit = Physics2D.Raycast(origin, _currentDirection, wallCheckDistance, wallLayerMask);
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
        }


        // 이동 전에 방향에 따라 스프라이트 좌우반전
        private void FlipSpirte()
        {
            if (_spriteRenderer != null && _currentDirection.x != 0)
            {
                _spriteRenderer.flipX = inverse ? _currentDirection.x < 0 : _currentDirection.x > 0;
            }
        }

        public void MoveTo(Vector3 direction, float duration, string moveType, bool inverse)
        {
            _currentDirection = direction.normalized;
            this.duration = duration;
            elapsedTime = 0;
            this.moveType = moveType;
            this.inverse = inverse;
        }

        public void Stop()
        {
            _currentDirection = Vector2.zero;
            // 만약 velocity 기반이면: rb.velocity = Vector2.zero;
        }

        // 넉백 코루틴
        public IEnumerator StepKnockback(Vector2 direction, float distance, float duration, int steps = 4)
        {
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
                RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, stepDist, wallLayerMask);
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
    }
}