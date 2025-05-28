using UnityEngine;

public class DummyPlayerController : MonoBehaviour
{
    float _moveSpeed = 10f;
    PlayerMove _playerMove;

    void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
    }

    void FixedUpdate()
    {
        _playerMove.Move(_moveSpeed);
    }
}