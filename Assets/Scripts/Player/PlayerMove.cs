using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    float _damping = 0.8f;
    Rigidbody2D _rb2d;

    void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    public void Move(float speed) 
    {
        Vector2 inputValue = Managers.Input.MoveInput;

        if (inputValue != Vector2.zero)
        {
            _rb2d.linearVelocity = inputValue * speed;
        }
        else 
        {
            _rb2d.linearVelocity *= _damping;
        }
    }
}