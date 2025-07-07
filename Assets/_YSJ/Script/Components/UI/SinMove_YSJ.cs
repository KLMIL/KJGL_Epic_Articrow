using UnityEngine;

public class SinMove_YSJ : MonoBehaviour
{
    public float Speed = 1.0f;
    public float valueMultifly = 1.0f;
    float sinValue;

    Vector2 defaultPosition;
    public Vector2 MoveDirection;

    private void Awake()
    {
        defaultPosition = transform.localPosition;
    }

    void Update()
    {
        this.sinValue += Time.deltaTime * Speed;
        float sinValue = Mathf.Sin(this.sinValue) * valueMultifly;
        transform.localPosition = defaultPosition + MoveDirection * sinValue;
    }
}
