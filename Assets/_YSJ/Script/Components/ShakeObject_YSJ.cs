using UnityEngine;

public class ShakeObject_YSJ : MonoBehaviour
{
    public float Strength;
    public float Range;

    Vector3 currentPosition;

    private void OnEnable()
    {
        currentPosition = transform.position;
    }

    void Update()
    {
        Shake();
    }

    void Shake() 
    {
        Vector3 RandomShakePos = new Vector3(Random.Range(-Range, Range), Random.Range(-Range, Range), 0);
        transform.position = currentPosition + RandomShakePos * Strength;
    }

    private void OnDisable()
    {
        transform.localPosition = Vector3.zero;
    }
}
