using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    EnemyController ownerController;
    Rigidbody2D rb;
    SpriteRenderer _spriteRenderer;

    public float moveSpeedMultiply = 1.0f;
    Vector2 _currentDirection = Vector2.zero;

    float duration = 0f;
    float elapsedTime = 0f;
    public float wallCheckDistance = 0.5f;
    string moveType;

    public LayerMask wallLayerMask;      // 벽 감지용 (필수)

    private void Awake()
    {
        ownerController = GetComponent<EnemyController>();
        rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (_currentDirection == Vector2.zero) return;
        // 좌우 이동 방향에 따라 flipX 결정


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
            if (elapsedTime < duration)
            {
                Vector2 origin = rb.position + _currentDirection * 0.1f;
                RaycastHit2D hit = Physics2D.Raycast(origin, _currentDirection, wallCheckDistance, wallLayerMask);
                if (hit.collider != null)
                {
                    Stop();
                    return;
                }
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
    }


    // 임시로, 이동 전에 방향에 따라 회전시킬 함수
    private void FlipSpirte()
    {
        if (_currentDirection.x != 0)
        {
            _spriteRenderer.flipX = _currentDirection.x > 0;
        }
    }

    public void MoveTo(Vector3 direction, float duration, string moveType)
    {
        _currentDirection = direction.normalized;
        this.duration = duration;
        elapsedTime = 0;
        this.moveType = moveType;
    }

    public void Stop()
    {
        _currentDirection = Vector2.zero;
        // 만약 velocity 기반이면: rb.velocity = Vector2.zero;
    }
}
