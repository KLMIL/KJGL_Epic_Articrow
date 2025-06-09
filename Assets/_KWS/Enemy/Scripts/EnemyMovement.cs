using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    EnemyController ownerController;

    public float moveSpeedMultiply = 1.0f;
    Vector3 _currentDirection = Vector3.zero;

    float duration = 0f;
    float elapsedTime = 0f;
    public float wallCheckDistance = 0.5f;
    string moveType;


    private void Start()
    {
        ownerController = GetComponent<EnemyController>();
    }

    private void Update()
    {
        if (_currentDirection == Vector3.zero) return;

        // 한 프레임만 이동하는 함수들
        if (moveType == "SimpleChase")
        {
            ownerController.transform.Translate(_currentDirection * ownerController.Status.moveSpeed * Time.deltaTime);
            return;
        }
        else if (moveType == "SmartChase")
        {
            ownerController.transform.Translate(_currentDirection * ownerController.Status.moveSpeed * Time.deltaTime);
            return;
        }


        elapsedTime += Time.deltaTime;

        if (moveType == "Normal")
        {
            if (Physics.Raycast(transform.position, _currentDirection, wallCheckDistance))
            {
                Stop();
                return;
            }

            if (elapsedTime < duration)
            {
                transform.position += _currentDirection * ownerController.Status.moveSpeed * Time.deltaTime;
            }
            else
            {
                Stop();
            }
        }
        else if (moveType == "SimpleMoveAway" || moveType == "SmartMoveAway")
        {
            if (elapsedTime < duration)
            {
                if (Physics.Raycast(transform.position, _currentDirection, wallCheckDistance))
                {
                    Stop();
                    return;
                }
                transform.position += _currentDirection * ownerController.Status.moveSpeed * Time.deltaTime;
            }
            else
            {
                Stop();
            }
        }

        if (moveType == "RushAttack")
        {
            //Debug.Log("Called Here");

            // 테스트용 wallcheck distance
            wallCheckDistance = 1.0f;

            // Rush 종료 조건(예시: 벽 충돌, 일정 거리 등)
            if (CheckRushEndCondition())
            {
                Stop();
                ownerController.ForceToNextState(); // Controller에 Rush 종료 신호
            }

            // 이동
            transform.position += ownerController.rushDirection * ownerController.Status.moveSpeed * ownerController.rushSpeedMultiply * Time.deltaTime;
        }
    }

    private bool CheckRushEndCondition()
    {
        return Physics.Raycast(transform.position, ownerController.rushDirection, wallCheckDistance)
            || elapsedTime > duration
            || ownerController.isRushAttacked;
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
        _currentDirection = Vector3.zero;
    }
}