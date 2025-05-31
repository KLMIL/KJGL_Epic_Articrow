using UnityEngine;

namespace BMC
{
    public class DummyPlayerController : MonoBehaviour
    {
        float _moveSpeed = 10f;
        PlayerMove _playerMove;

        void Awake()
        {
            _playerMove = GetComponent<PlayerMove>();
        }

        void Start()
        {
            transform.SetParent(null);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                MapManager.Instance.CurrentRoom.Complete();
            }
        }

        void FixedUpdate()
        {
            _playerMove.Move();
        }
    }
}